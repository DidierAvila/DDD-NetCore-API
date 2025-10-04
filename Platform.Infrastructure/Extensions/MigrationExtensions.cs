using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Platform.Infrastructure.DbContexts;

namespace Platform.Infrastructure.Extensions
{
    public static class MigrationExtensions
    {
        /// <summary>
        /// Aplica las migraciones pendientes a la base de datos
        /// </summary>
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<PlatformDbContext>();
                    context.Database.Migrate();
                    
                    var logger = services.GetRequiredService<ILogger<PlatformDbContext>>();
                    logger.LogInformation("Base de datos migrada correctamente");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<PlatformDbContext>>();
                    logger.LogError(ex, "Error al migrar la base de datos");
                    throw;
                }
            }
            
            return host;
        }
    }
}