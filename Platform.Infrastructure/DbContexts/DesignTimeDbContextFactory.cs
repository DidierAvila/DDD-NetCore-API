using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Platform.Infrastructure.DbContexts
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PlatformDbContext>
    {
        public PlatformDbContext CreateDbContext(string[] args)
        {
            // Obtener la ruta del directorio del proyecto
            var basePath = Directory.GetCurrentDirectory();
            
            // Construir la ruta al archivo de configuración
            var configPath = Path.Combine(basePath, "..", "Platform.Api", "appsettings.Development.json");
            
            // Crear la configuración
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(configPath, optional: false)
                .Build();
            
            // Obtener la cadena de conexión
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            // Crear las opciones del DbContext
            var optionsBuilder = new DbContextOptionsBuilder<PlatformDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            
            return new PlatformDbContext(optionsBuilder.Options);
        }
    }
}