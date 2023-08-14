// ------------------------------------------------------------------------------------------------
//  <copyright file="BikeRepositoryTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.InfrastructureTests.Persistance.Repositories;

using System;
using System.Diagnostics.CodeAnalysis;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Persistence;
using Persistence.Repositories;
using Persistence.Repositories.Abstractions;
using Xunit;

public class BikeRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;

    public BikeRepositoryTests()
    {
        this._context = ApplicationDbContextTestFactory.Create();
    }
    
    public void Dispose()
    {
        this._context.Dispose();
    }
    
    
    [Fact]
    public void Ctor_Should_ThrowsExceptionWhenDbContextIsNull()
    {
        // Arrange & Act &Assert
        Assert.Throws<ArgumentNullException>(() => new BikeRepository(null!));
    }
    
    [Fact]
    public void Add_ShouldAddBikeToDbContext()
    {
        // Arrange
        var repository = new BikeRepository(this._context);
        var bike = Bike.Create("Test", Guid.NewGuid(), 8.00m, "Brand",
            "Model", "Notes", TestsContants.UserUserId).Value;
        
        // Act
        repository.Add(bike);
        
        // Assert
        Assert.Equal("Test", bike.Name);
    }
    
    [Fact]
    public async Task GetAllByUserAsync_ShouldReturnCollection()
    {
        // Arrange
        var repository = new BikeRepository(this._context);
        
        // Act
        var result = await repository.GetAllByUserAsync(TestsContants.UserUserId);
        
        // Assert
        Assert.Equal(3, result.Count);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecordWhenExists()
    {
        // Arrange
        var repository = new BikeRepository(this._context);
        var bikeId = this._context.Set<Bike>().First().Id;
        
        // Act
        var result = await repository.GetByIdAsync(bikeId, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNullWhenDoesNotExists()
    {
        // Arrange
        var repository = new BikeRepository(this._context);
        var bikeId = Guid.NewGuid();
        
        // Act
        var result = await repository.GetByIdAsync(bikeId, CancellationToken.None);
        
        // Assert
        Assert.Null(result);
    }
    
    // [Fact]
    // public async Task Delete_ShouldDeleteBikeToDbContext()
    // {
    //     // Arrange
    //     var repository = new BikeRepository(this._context);
    //     var bikeId = this._context.Set<Bike>().First().Id;
    //     
    //     // Act
    //     var result = await repository.DeleteAsync(bikeId, CancellationToken.None);
    //
    //     // Assert
    //     Assert.True(result);
    // }
    
    [Fact]
    public async Task Delete_ShouldNotDeleteBikeToDbContext()
    {
        // Arrange
        var repository = new BikeRepository(this._context);

        // Act
        var result = await repository.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void Update_ShouldDeleteBikeToDbContext()
    {
        // Arrange
        var repository = new BikeRepository(this._context);
        var bike = this._context.Set<Bike>().First();

        // Act
        bike.Update("changed", Guid.NewGuid(), 9.99M, "changed", "changed", null, TestsContants.UserUserId);
        repository.Update(bike);

        // Assert
        Assert.NotNull(bike);
    }
    
    
}