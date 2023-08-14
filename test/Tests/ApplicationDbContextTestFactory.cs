// ------------------------------------------------------------------------------------------------
//  <copyright file="ApplicationDbContextFactory.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests;

using System.Collections.Generic;
using Common.Constants;
using Common.Enumerations;
using Domain.Entities;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

public static class ApplicationDbContextTestFactory
{
    public static ApplicationDbContext Create()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .AddInterceptors();
        var context = new ApplicationDbContext(optionsBuilder.Options);

        var roles = new List<Role>()
        {
            new Role() { Id = TestsContants.UserRoleId, Name = "User", NormalizedName = "USER" },
            new Role() { Id = TestsContants.AdministratorRoleId, Name = "Admin", NormalizedName = "ADMIN" }
        };

        var users = new List<User>()
        {
            new User()
            {
                Id = TestsContants.UserUserId, UserName = "user",
                FirstName = "Ivan", LastName = "Ivanov",
                Email = "test@test.com",
                UserRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        UserId = TestsContants.UserUserId, 
                        RoleId = TestsContants.UserRoleId
                    } 
                }
            },
            new User()
            {
                Id = TestsContants.AdministratorUserId, UserName = "admin",
                FirstName = "Petar", LastName = "Petrov",
                Email = "admin@test.com",
                UserRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        UserId = TestsContants.AdministratorUserId, 
                        RoleId = TestsContants.AdministratorRoleId
                    }
                }
            }
        };
        
        var types = new List<BikeType>()
        {
            BikeType.Create("Road bike", false).Value,
            BikeType.Create("Mountain bike", false).Value
        };

        var bikes= new List<Bike>()
        {
            Bike.Create("name", types.First().Id, 10.00m, "brand", "model", 
                "notes", TestsContants.UserUserId).Value,
            Bike.Create("name", types.First().Id, 8.00m, "brand", "model", 
                "notes", TestsContants.UserUserId).Value,
            Bike.Create("name", types.Last().Id, 12.00m, "brand", "model", 
                "notes", Guid.NewGuid()).Value,
            Bike.Create("name", types.Last().Id, 6.80m, "brand", "model", 
                "notes", TestsContants.UserUserId).Value
        };

        var activities = new List<Activity>()
        {
            Activity.Create("Trip 1", "Description", "Private", 10.00M,
                new TimeSpan(0, 1, 0, 0), 100m, 99m,
                VisibilityLevelType.All, new DateTime(2023, 8, 5), bikes.First().Id, TestsContants.UserUserId).Value,
            Activity.Create("Trip 2", "Description", null, 10.00M,
                new TimeSpan(0, 1, 0, 0), 100m, 99m,
                VisibilityLevelType.All, new DateTime(2023, 8, 6), bikes.First().Id, TestsContants.UserUserId).Value,
            Activity.Create("Trip 3", "Description", "Private", 10.00M,
                new TimeSpan(0, 1, 0, 0), 100m, 99m,
                VisibilityLevelType.All, new DateTime(2023, 8, 7), bikes.First().Id, TestsContants.UserUserId).Value,
            Activity.Create("Trip 4", "Description", "Private", 10.00M,
                new TimeSpan(0, 1, 0, 0), 100m, 99m,
                VisibilityLevelType.All, new DateTime(2023, 8, 1), bikes.First().Id, TestsContants.UserUserId).Value
        };

        var activityLike = new ActivityLike() { Activity = activities.First(), UserId = TestsContants.UserUserId };

        var comments = new List<Comment>()
        {
            Comment.Create(activities.First().Id, TestsContants.UserUserId, "Comment content").Value,
            Comment.Create(activities.First().Id, TestsContants.UserUserId, "Comment content").Value,
        };
        
        context.Set<Role>().AddRange(roles);
        context.Set<BikeType>().AddRange(types);
        context.Set<Bike>().AddRange(bikes);
        context.Set<Activity>().AddRange(activities);
        context.Set<ActivityLike>().Add(activityLike);
        context.Set<Comment>().AddRange(comments);
        context.Set<User>().AddRange(users);
        
        context.SaveChanges();
        return context;
    }
}