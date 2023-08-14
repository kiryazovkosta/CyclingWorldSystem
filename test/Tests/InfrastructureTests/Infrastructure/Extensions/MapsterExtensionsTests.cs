// ------------------------------------------------------------------------------------------------
//  <copyright file="MapsterExtensionsTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.InfrastructureTests.Infrastructure.Extensions;

using Application.Entities.Activities.Models;
using Application.Entities.GpxFiles.Models;
using Application.Identity.Users.Commands.CreateUser;
using Common.Enumerations;
using Domain.Entities;
using Domain.Entities.GpxFile;
using Domain.Identity;
using global::Infrastructure.Extensions;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class MapsterExtensionsTests
{
    [Fact]
        public void RegisterMapsterConfiguration_RegistersGpxTrkTrkptToWaypointResponseMapping()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterMapsterConfiguration();

            // Assert
            var source = new GpxTrkTrkpt();
            var destination = source.Adapt<WaypointResponse>();
            Assert.NotNull(destination);
        }

        [Fact]
        public void RegisterMapsterConfiguration_RegistersCreateUserCommandToUserMapping()
        {
            // Arrange
            var servicesMock = new Mock<IServiceCollection>();
        
            // Act
            servicesMock.Object.RegisterMapsterConfiguration();
        
            // Assert
            var source = new CreateUserCommand("UserName", "test@test.com", "123456", "123456", "FirstName", "MiddleName",
                "LastName", null);
            var destination = source.Adapt<User>();
            Assert.NotNull(destination);
        }
        
        [Fact]
        public void RegisterMapsterConfiguration_RegistersActivityToSimplyActivityResponseMapping()
        {
            // Arrange
            var servicesMock = new Mock<IServiceCollection>();
        
            // Act
            servicesMock.Object.RegisterMapsterConfiguration();
        
            // Assert
            var source = Activity.Create("title", "description", null, 10m, new TimeSpan(100), null, null,
                VisibilityLevelType.All, DateTime.Now, Guid.NewGuid(), Guid.NewGuid()).Value;
            var destination = source.Adapt<SimplyActivityResponse>();
            Assert.NotNull(destination);
        }
        
        [Fact]
        public void RegisterMapsterConfiguration_RegistersActivityToActivityResponseMapping()
        {
            // Arrange
            var servicesMock = new Mock<IServiceCollection>();
        
            // Act
            servicesMock.Object.RegisterMapsterConfiguration();
        
            // Assert
            var source = Activity.Create("title", "description", null, 10m, new TimeSpan(100), null, null,
                VisibilityLevelType.All, DateTime.Now, Guid.NewGuid(), Guid.NewGuid()).Value;
            var destination = source.Adapt<ActivityResponse>();
            Assert.NotNull(destination);
        }
}