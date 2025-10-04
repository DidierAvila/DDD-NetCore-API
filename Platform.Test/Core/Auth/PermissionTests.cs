using Microsoft.EntityFrameworkCore;
using Platform.Domain.Entities.Auth;
using Platform.Infrastructure.DbContexts;
using Platform.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Platform.Test.Core.Auth
{
    public class PermissionTests : TestBase
    {
        [Fact]
        public async Task Create_Permission_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<Permission>(context, GetMockLogger<Permission>());
            
            var permission = new Permission
            {
                Id = Guid.NewGuid(),
                Name = "Test Permission",
                Description = "Test Permission Description",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            
            // Act
            await repository.Create(permission, cancellationToken);
            
            // Assert
            var result = await context.Permissions.FindAsync(permission.Id);
            Assert.NotNull(result);
            Assert.Equal(permission.Id, result.Id);
            Assert.Equal("Test Permission", result.Name);
        }
        
        [Fact]
        public async Task Get_Permission_By_Id_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<Permission>(context, GetMockLogger<Permission>());
            
            var permissionId = Guid.NewGuid();
            var permission = new Permission
            {
                Id = permissionId,
                Name = "Test Permission",
                Description = "Test Permission Description",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            
            await context.Permissions.AddAsync(permission);
            await context.SaveChangesAsync();
            
            // Act
            var result = await repository.GetByID(permissionId, cancellationToken);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(permissionId, result.Id);
            Assert.Equal("Test Permission", result.Name);
        }
        
        [Fact]
        public async Task Update_Permission_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<Permission>(context, GetMockLogger<Permission>());
            
            var permissionId = Guid.NewGuid();
            var permission = new Permission
            {
                Id = permissionId,
                Name = "Test Permission",
                Description = "Test Permission Description",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            
            await context.Permissions.AddAsync(permission);
            await context.SaveChangesAsync();
            
            // Act
            var existingPermission = await repository.GetByID(permissionId, cancellationToken);
            existingPermission.Name = "Updated Permission";
            existingPermission.Description = "Updated Description";
            await repository.Update(existingPermission, cancellationToken);
            
            // Assert
            var result = await context.Permissions.FindAsync(permissionId);
            Assert.NotNull(result);
            Assert.Equal(permissionId, result.Id);
            Assert.Equal("Updated Permission", result.Name);
            Assert.Equal("Updated Description", result.Description);
        }
        
        [Fact]
        public async Task Delete_Permission_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<Permission>(context, GetMockLogger<Permission>());
            
            var permissionId = Guid.NewGuid();
            var permission = new Permission
            {
                Id = permissionId,
                Name = "Test Permission",
                Description = "Test Permission Description",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            
            await context.Permissions.AddAsync(permission);
            await context.SaveChangesAsync();
            
            // Act
            await repository.Delete(permission, cancellationToken);
            
            // Assert
            var deletedPermission = await context.Permissions.FindAsync(permissionId);
            Assert.Null(deletedPermission);
        }
        
        [Fact]
        public async Task Get_All_Permissions_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<Permission>(context, GetMockLogger<Permission>());
            
            var permissions = new List<Permission>
            {
                new Permission
                {
                    Id = Guid.NewGuid(),
                    Name = "Permission 1",
                    Description = "Description 1",
                    Status = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Permission
                {
                    Id = Guid.NewGuid(),
                    Name = "Permission 2",
                    Description = "Description 2",
                    Status = true,
                    CreatedAt = DateTime.UtcNow
                }
            };
            
            await context.Permissions.AddRangeAsync(permissions);
            await context.SaveChangesAsync();
            
            // Act
            var result = await repository.GetAll(cancellationToken);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}