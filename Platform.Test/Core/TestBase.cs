using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Platform.Infrastructure.DbContexts;
using Platform.Infrastructure.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Platform.Test.Core
{
    public abstract class TestBase
    {
        protected readonly Mock<IMapper> MockMapper;
        
        protected TestBase()
        {
            MockMapper = new Mock<IMapper>();
        }
        
        protected DbContextOptions<PlatformDbContext> CreateNewInMemoryDatabase()
        {
            return new DbContextOptionsBuilder<PlatformDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
        }
        
        protected static CancellationToken GetCancellationToken()
        {
            return CancellationToken.None;
        }
        
        protected ILogger<RepositoryBase<TEntity>> GetMockLogger<TEntity>() where TEntity : class
        {
            return new Mock<ILogger<RepositoryBase<TEntity>>>().Object;
        }
        
        // Método para crear un contexto de base de datos para pruebas que no use SetCommandTimeout
        protected PlatformDbContext CreateTestDbContext()
        {
            var options = CreateNewInMemoryDatabase();
            return new TestPlatformDbContext(options);
        }
    }
    
    // Clase derivada de PlatformDbContext para pruebas que sobrescribe el método SetCommandTimeout
    public class TestPlatformDbContext : PlatformDbContext
    {
        public TestPlatformDbContext(DbContextOptions<PlatformDbContext> options) : base(options)
        {
        }
    }
}