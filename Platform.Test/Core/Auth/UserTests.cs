using Microsoft.EntityFrameworkCore;
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
    public class UserTests : TestBase
    {
        [Fact]
        public async Task Create_User_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<User>(context, GetMockLogger<User>());
            
            // Crear un UserType para la prueba
            var userType = new UserType
            {
                Id = Guid.NewGuid(),
                Name = "Test User Type",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            await context.UserTypes.AddAsync(userType);
            await context.SaveChangesAsync();
            
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Email = "test@example.com",
                Status = true,
                UserTypeId = userType.Id,
                CreatedAt = DateTime.UtcNow,
                ExtraData = "{}"
            };
            
            // Act
            await repository.Create(user, cancellationToken);
            
            // Assert
            var result = await context.Users.FindAsync(user.Id);
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("test@example.com", result.Email);
        }
        
        [Fact]
        public async Task Get_User_By_Id_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<User>(context, GetMockLogger<User>());
            
            // Crear un UserType para la prueba
            var userType = new UserType
            {
                Id = Guid.NewGuid(),
                Name = "Test User Type",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Name = "Test User",
                Email = "test@example.com",
                Status = true,
                UserTypeId = userType.Id,
                CreatedAt = DateTime.UtcNow,
                ExtraData = "{}"
            };
            
            await context.UserTypes.AddAsync(userType);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            
            // Act
            var result = await repository.GetByID(userId, cancellationToken);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("test@example.com", result.Email);
        }
        
        [Fact]
        public async Task Update_User_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<User>(context, GetMockLogger<User>());
            
            // Crear un UserType para la prueba
            var userType = new UserType
            {
                Id = Guid.NewGuid(),
                Name = "Test User Type",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Name = "Test User",
                Email = "test@example.com",
                Status = true,
                UserTypeId = userType.Id,
                CreatedAt = DateTime.UtcNow,
                ExtraData = "{}"
            };
            
            await context.UserTypes.AddAsync(userType);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            
            // Act
            var existingUser = await repository.GetByID(userId, cancellationToken);
            existingUser.Name = "Updated User";
            existingUser.Email = "updated@example.com";
            await repository.Update(existingUser, cancellationToken);
            
            // Assert
            var result = await context.Users.FindAsync(userId);
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("Updated User", result.Name);
            Assert.Equal("updated@example.com", result.Email);
        }
        
        [Fact]
        public async Task Delete_User_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<User>(context, GetMockLogger<User>());
            
            // Crear un UserType para la prueba
            var userType = new UserType
            {
                Id = Guid.NewGuid(),
                Name = "Test User Type",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Name = "Test User",
                Email = "test@example.com",
                Status = true,
                UserTypeId = userType.Id,
                CreatedAt = DateTime.UtcNow,
                ExtraData = "{}"
            };
            
            await context.UserTypes.AddAsync(userType);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            
            // Act
            await repository.Delete(user, cancellationToken);
            
            // Assert
            var deletedUser = await context.Users.FindAsync(userId);
            Assert.Null(deletedUser);
        }
        
        [Fact]
        public async Task Get_All_Users_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<User>(context, GetMockLogger<User>());
            
            // Crear un UserType para la prueba
            var userType = new UserType
            {
                Id = Guid.NewGuid(),
                Name = "Test User Type",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "User 1",
                    Email = "test1@example.com",
                    Status = true,
                    UserTypeId = userType.Id,
                    CreatedAt = DateTime.UtcNow,
                    ExtraData = "{}"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "User 2",
                    Email = "test2@example.com",
                    Status = true,
                    UserTypeId = userType.Id,
                    CreatedAt = DateTime.UtcNow,
                    ExtraData = "{}"
                }
            };
            
            await context.UserTypes.AddAsync(userType);
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
            
            // Act
            var result = await repository.GetAll(cancellationToken);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}