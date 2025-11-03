using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Platform.Domain.Repositories;
using Platform.Infrastructure.DbContexts;

namespace Platform.Infrastructure.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        internal readonly PlatformDbContext _context;
        private readonly ILogger<RepositoryBase<TEntity>> _logger;
        
        public RepositoryBase(PlatformDbContext context, ILogger<RepositoryBase<TEntity>> logger)
        {
            _context = context;
            _logger = logger;

            _context.Database.SetCommandTimeout(180); // Establece el timeout a 180 segundos (3 minutos)
        }

        internal DbSet<TEntity> EntitySet => _context.Set<TEntity>();

        public async Task<TEntity?> Delete(int id, CancellationToken cancellationToken)
        {
            TEntity? entity = await EntitySet.FindAsync(id, cancellationToken);
            if (entity != null)
            {
                EntitySet.Remove(entity);
                await SaveChangesWithRetryAsync(cancellationToken);
            }
            return entity;
        }

        public async Task<TEntity?> Delete(Guid id, CancellationToken cancellationToken)
        {
            TEntity? entity = await EntitySet.FindAsync(id, cancellationToken);
            if (entity != null)
            {
                EntitySet.Remove(entity);
                await SaveChangesWithRetryAsync(cancellationToken);
            }
            return entity;
        }

        public async Task Delete(TEntity entity, CancellationToken cancellationToken)
        {
            EntitySet.Remove(entity);
            await SaveChangesWithRetryAsync(cancellationToken);
        }

        public async Task<TEntity?> Find(Expression<Func<TEntity, bool>> expr, CancellationToken cancellationToken)
        {
            try
            {
                // Cargar todos los datos en memoria y luego aplicar el filtro
                var results = await _context.Set<TEntity>().ToListAsync(cancellationToken);
                _logger.LogInformation($"Find: Cargados {results.Count} registros de tipo {typeof(TEntity).Name} en memoria");
                
                // Aplicar el filtro en memoria
                var result = results.AsQueryable().FirstOrDefault(expr.Compile());
                
                if (result != null)
                {
                    _logger.LogInformation($"Find: Se encontró un registro de tipo {typeof(TEntity).Name}");
                }
                else
                {
                    _logger.LogWarning($"Find: No se encontró ningún registro de tipo {typeof(TEntity).Name} con el filtro especificado");
                }
                
                return result;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Database operation was canceled while finding entity of type {EntityType}. This may be due to timeout or connection issues.", typeof(TEntity).Name);
                throw new InvalidOperationException($"La operación de búsqueda fue cancelada. Esto puede deberse a problemas de conexión o timeout de la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while finding entity of type {EntityType}", typeof(TEntity).Name);
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>?> Finds(Expression<Func<TEntity, bool>> expr, CancellationToken cancellationToken)
        {
            try
            {
                return await EntitySet.AsNoTracking().Where(expr).ToListAsync(cancellationToken);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Database operation was canceled while finding multiple entities of type {EntityType}. This may be due to timeout or connection issues.", typeof(TEntity).Name);
                throw new InvalidOperationException($"La operación de búsqueda múltiple fue cancelada. Esto puede deberse a problemas de conexión o timeout de la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while finding multiple entities of type {EntityType}", typeof(TEntity).Name);
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                // Cargar directamente en memoria para evitar problemas con SQLite
                var users = await EntitySet.ToListAsync();
                var result = await _context.Users.ToListAsync(cancellationToken);
                _logger.LogInformation($"GetAll: Se obtuvieron {result.Count} entidades de tipo {typeof(TEntity).Name}");
                
                // Si no hay resultados, registrar una advertencia
                if (result.Count == 0)
                {
                    _logger.LogWarning($"GetAll: No se encontraron entidades de tipo {typeof(TEntity).Name}");
                }
                
                return (IEnumerable<TEntity>)result;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Database operation was canceled while getting all entities of type {EntityType}. This may be due to timeout or connection issues.", typeof(TEntity).Name);
                throw new InvalidOperationException($"La operación de obtener todas las entidades fue cancelada. Esto puede deberse a problemas de conexión o timeout de la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las entidades de tipo {EntityType}: {Message}", typeof(TEntity).Name, ex.Message);
                throw;
            }
        }

        public async Task<TEntity?> GetByID(Guid id, CancellationToken cancellationToken)
        {
            return await EntitySet.FindAsync(id, cancellationToken);
        }

        public async Task<TEntity?> GetByID(int id, CancellationToken cancellationToken)
        {
            return await EntitySet.FindAsync(id, cancellationToken);
        }

        public async Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken)
        {
            var result = await EntitySet.AddAsync(entity, cancellationToken);
            await SaveChangesWithRetryAsync(cancellationToken);
            return result.Entity;
        }

        public async Task Update(TEntity entity, CancellationToken cancellationToken)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await SaveChangesWithRetryAsync(cancellationToken);
        }

        public void GetByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        // Método para guardar cambios con reintentos para SQLite
        private async Task SaveChangesWithRetryAsync(CancellationToken cancellationToken)
        {
            int retryCount = 0;
            const int maxRetries = 3;
            const int retryDelayMs = 200;
            
            while (true)
            {
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    return;
                }
                catch (DbUpdateException ex) when (IsSQLiteLockException(ex) && retryCount < maxRetries)
                {
                    retryCount++;
                    _logger.LogWarning(ex, "SQLite lock detected while saving changes. Retry attempt {RetryCount} of {MaxRetries}", 
                        retryCount, maxRetries);
                    await Task.Delay(retryDelayMs * retryCount, cancellationToken);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        // Método auxiliar para detectar excepciones de bloqueo de SQLite
        private bool IsSQLiteLockException(Exception ex)
        {
            // Verificar si es una excepción de bloqueo de SQLite
            return ex.Message.Contains("database is locked") || 
                   ex.Message.Contains("busy") ||
                   ex.Message.Contains("cannot start a transaction");
        }
    }
}
