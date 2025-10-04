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
    public class UserTypeTests : TestBase
    {
        [Fact]
        public async Task Create_UserType_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<UserType>(context, GetMockLogger<UserType>());
            
            var userType = new UserType
            {
                Id = Guid.NewGuid(),
                Name = "Test UserType",
                Description = "Test UserType Description",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            
            // Act
            await repository.Create(userType, cancellationToken);
            
            // Assert
            var result = await context.UserTypes.FindAsync(userType.Id);
            Assert.NotNull(result);
            Assert.Equal(userType.Id, result.Id);
            Assert.Equal("Test UserType", result.Name);
        }
        
        [Fact]
        public async Task Get_UserType_By_Id_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<UserType>(context, GetMockLogger<UserType>());
            
            var userTypeId = Guid.NewGuid();
            var userType = new UserType
            {
                Id = userTypeId,
                Name = "Test UserType",
                Description = "Test UserType Description",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            
            await context.UserTypes.AddAsync(userType);
            await context.SaveChangesAsync();
            
            // Act
            var result = await repository.GetByID(userTypeId, cancellationToken);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(userTypeId, result.Id);
            Assert.Equal("Test UserType", result.Name);
        }
        
        [Fact]
        public async Task Update_UserType_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<UserType>(context, GetMockLogger<UserType>());
            
            var userTypeId = Guid.NewGuid();
            var userType = new UserType
            {
                Id = userTypeId,
                Name = "Test UserType",
                Description = "Test UserType Description",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            
            await context.UserTypes.AddAsync(userType);
            await context.SaveChangesAsync();
            
            // Act
            var existingUserType = await repository.GetByID(userTypeId, cancellationToken);
            existingUserType.Name = "Updated UserType";
            existingUserType.Description = "Updated Description";
            await repository.Update(existingUserType, cancellationToken);
            
            // Assert
            var result = await context.UserTypes.FindAsync(userTypeId);
            Assert.NotNull(result);
            Assert.Equal(userTypeId, result.Id);
            Assert.Equal("Updated UserType", result.Name);
            Assert.Equal("Updated Description", result.Description);
        }
        
        [Fact]
        public async Task Delete_UserType_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<UserType>(context, GetMockLogger<UserType>());
            
            var userTypeId = Guid.NewGuid();
            var userType = new UserType
            {
                Id = userTypeId,
                Name = "Test UserType",
                Description = "Test UserType Description",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            
            await context.UserTypes.AddAsync(userType);
            await context.SaveChangesAsync();
            
            // Act
            await repository.Delete(userType, cancellationToken);
            
            // Assert
            var deletedUserType = await context.UserTypes.FindAsync(userTypeId);
            Assert.Null(deletedUserType);
        }
        
        [Fact]
        public async Task Get_All_UserTypes_Success()
        {
            // Arrange
            var options = CreateNewInMemoryDatabase();
            var cancellationToken = GetCancellationToken();
            
            using var context = new PlatformDbContext(options);
            var repository = new RepositoryBase<UserType>(context, GetMockLogger<UserType>());
            
            var userTypes = new List<UserType>
            {
                new UserType
                {
                    Id = Guid.NewGuid(),
                    Name = "UserType 1",
                    Description = "Description 1",
                    Status = true,
                    CreatedAt = DateTime.UtcNow
                },
                new UserType
                {
                    Id = Guid.NewGuid(),
                    Name = "UserType 2",
                    Description = "Description 2",
                    Status = true,
                    CreatedAt = DateTime.UtcNow
                }
            };
            
            await context.UserTypes.AddRangeAsync(userTypes);
            await context.SaveChangesAsync();
            
            // Act
            var result = await repository.GetAll(cancellationToken);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}