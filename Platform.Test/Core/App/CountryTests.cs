using Microsoft.EntityFrameworkCore;
using Platform.Domain.Entities.App;
using Platform.Infrastructure.DbContexts;
using Platform.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Platform.Test.Core.App
{
    public class CountryTests : TestBase
    {
        [Fact]
        public async Task Create_Country_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<Country>(context, GetMockLogger<Country>());
            
            var country = new Country
            {
                Alpha2Code = "TC",
                Name = "Test Country",
                Alpha3Code = "TCY",
                Capital = "Test Capital",
                Region = "Test Region"
            };
            
            // Act
            await repository.Create(country, cancellationToken);
            
            // Assert
            var result = await context.Countries.FindAsync(country.Alpha2Code);
            Assert.NotNull(result);
            Assert.Equal("TC", result.Alpha2Code);
            Assert.Equal("Test Country", result.Name);
        }
        
        [Fact]
        public async Task Get_Country_By_Id_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<Country>(context, GetMockLogger<Country>());
            
            var country = new Country
            {
                Alpha2Code = "TC",
                Name = "Test Country",
                Alpha3Code = "TCY",
                Capital = "Test Capital",
                Region = "Test Region"
            };
            
            await context.Countries.AddAsync(country);
            await context.SaveChangesAsync();
            
            // Act
            var result = await repository.Find(c => c.Alpha2Code == "TC", cancellationToken);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("TC", result.Alpha2Code);
            Assert.Equal("Test Country", result.Name);
        }
        
        [Fact]
        public async Task Update_Country_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            // Crear y guardar el país
            string alpha2Code = "TC";
            using (var context = new PlatformDbContext(options))
            {
                var country = new Country
                {
                    Alpha2Code = alpha2Code,
                    Name = "Test Country",
                    Alpha3Code = "TCY",
                    Capital = "Test Capital",
                    Region = "Test Region"
                };
                
                await context.Countries.AddAsync(country);
                await context.SaveChangesAsync();
            }
            
            // Act - Usar un contexto diferente para la actualización
            using (var context = new PlatformDbContext(options))
            {
                var repository = new RepositoryBase<Country>(context, GetMockLogger<Country>());
                var existingCountry = await context.Countries.FindAsync(alpha2Code);
                existingCountry.Name = "Updated Country";
                existingCountry.Capital = "Updated Capital";
                await repository.Update(existingCountry, cancellationToken);
            }
            
            // Assert - Usar un tercer contexto para verificar
            using (var context = new PlatformDbContext(options))
            {
                var result = await context.Countries.FindAsync(alpha2Code);
                Assert.NotNull(result);
                Assert.Equal(alpha2Code, result.Alpha2Code);
                Assert.Equal("Updated Country", result.Name);
                Assert.Equal("Updated Capital", result.Capital);
            }
        }
        
        [Fact]
        public async Task Delete_Country_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<Country>(context, GetMockLogger<Country>());
            
            var country = new Country
            {
                Alpha2Code = "TC",
                Name = "Test Country",
                Alpha3Code = "TCY",
                Capital = "Test Capital",
                Region = "Test Region"
            };
            
            await context.Countries.AddAsync(country);
            await context.SaveChangesAsync();
            
            // Act
            await repository.Delete(country, cancellationToken);
            
            // Assert
            var deletedCountry = await context.Countries.FindAsync(country.Alpha2Code);
            Assert.Null(deletedCountry);
        }
        
        [Fact]
        public async Task Get_All_Countries_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<Country>(context, GetMockLogger<Country>());
            
            var countries = new List<Country>
            {
                new Country
                {
                    Alpha2Code = "C1",
                    Name = "Country 1",
                    Alpha3Code = "CY1",
                    Capital = "Capital 1",
                    Region = "Region 1"
                },
                new Country
                {
                    Alpha2Code = "C2",
                    Name = "Country 2",
                    Alpha3Code = "CY2",
                    Capital = "Capital 2",
                    Region = "Region 2"
                }
            };
            
            await context.Countries.AddRangeAsync(countries);
            await context.SaveChangesAsync();
            
            // Act
            var result = await repository.GetAll(cancellationToken);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}