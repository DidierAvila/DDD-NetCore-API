using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;
using Platform.Infrastructure.DbContexts;
using Platform.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Platform.Test.Core.Auth
{
    public class RoleTests : TestBase
    {
        [Fact]
        public async Task Create_Role_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<Role>(context, GetMockLogger<Role>());
            
            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Test Role",
                Description = "Test Role Description",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            
            // Act
            await repository.Create(role, cancellationToken);
            
            // Assert
            var result = await context.Roles.FindAsync(role.Id);
            Assert.NotNull(result);
            Assert.Equal(role.Id, result.Id);
            Assert.Equal("Test Role", result.Name);
            Assert.Equal("Test Role Description", result.Description);
        }
        
        [Fact]
        public async Task Get_Role_By_Id_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<Role>(context, GetMockLogger<Role>());
            
            var roleId = Guid.NewGuid();
            var role = new Role
            {
                Id = roleId,
                Name = "Test Role",
                Description = "Test Role Description",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            
            await context.Roles.AddAsync(role);
            await context.SaveChangesAsync();
            
            // Act
            var result = await repository.GetByID(roleId, cancellationToken);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(roleId, result.Id);
            Assert.Equal(role.Name, result.Name);
            Assert.Equal(role.Description, result.Description);
        }
        
        [Fact]
        public async Task Update_Role_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<Role>(context, GetMockLogger<Role>());
            
            var roleId = Guid.NewGuid();
            var role = new Role
            {
                Id = roleId,
                Name = "Test Role",
                Description = "Test Role Description",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            
            await context.Roles.AddAsync(role);
            await context.SaveChangesAsync();
            
            // Act
            var existingRole = await repository.GetByID(roleId, cancellationToken);
            existingRole.Name = "Updated Role";
            existingRole.Description = "Updated Description";
            await repository.Update(existingRole, cancellationToken);
            
            // Assert
            var result = await context.Roles.FindAsync(roleId);
            Assert.NotNull(result);
            Assert.Equal(roleId, result.Id);
            Assert.Equal("Updated Role", result.Name);
            Assert.Equal("Updated Description", result.Description);
        }
        
        [Fact]
        public async Task Delete_Role_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<Role>(context, GetMockLogger<Role>());
            
            var roleId = Guid.NewGuid();
            var role = new Role
            {
                Id = roleId,
                Name = "Test Role",
                Description = "Test Role Description",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            
            await context.Roles.AddAsync(role);
            await context.SaveChangesAsync();
            
            // Act
            await repository.Delete(role, cancellationToken);
            
            // Assert
            var deletedRole = await context.Roles.FindAsync(roleId);
            Assert.Null(deletedRole);
        }
        
        [Fact]
        public async Task Get_All_Roles_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<Role>(context, GetMockLogger<Role>());
            
            var roles = new List<Role>
            {
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Role 1",
                    Description = "Description 1",
                    Status = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Role 2",
                    Description = "Description 2",
                    Status = true,
                    CreatedAt = DateTime.UtcNow
                }
            };
            
            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
            
            // Act
            var result = await repository.GetAll(cancellationToken);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}