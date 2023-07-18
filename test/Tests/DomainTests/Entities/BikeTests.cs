namespace Tests.DomainTests.Entities;

using Common.Constants;
using Domain.Entities;
using Domain.Errors;
using System;

public class BikeTests
{
    [Fact]
    public void Constructor_Should_CreateValidBike()
    {
        // Arrange
        string name = "Name";
        Guid type = Guid.NewGuid();
        decimal weight = 10.00M;
        string brand = "Brand";
        string model = "Model";
        string? notes = null;
        Guid user = Guid.NewGuid();

        // Act
        var result = Bike.Create(name, type, weight, brand, model, notes, user);
        var bike = result.Value;

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(name, bike.Name);
        Assert.Equal(type, bike.BikeTypeId);
        Assert.Equal(weight, bike.Weight);
        Assert.Equal(brand, bike.Brand);
        Assert.Equal(model, bike.Model);
        Assert.Null(bike.Notes);
        Assert.Equal(user, bike.UserId);
        Assert.Empty(bike.Activities);
        Assert.Null(bike.User);
        Assert.Null(bike.BikeType);
    }

    [Fact]
    public void Create_Should_ReturnsErrorWhenNameIsNullOrEmpty()
    {
        // Arrange
        string name = "";
        Guid type = Guid.NewGuid();
        decimal weight = 10.00M;
        string brand = "Brand";
        string model = "Model";
        string? notes = null;
        Guid user = Guid.NewGuid();

        // Act
        var result = Bike.Create(name, type, weight, brand, model, notes, user);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Bike.NameIsNullOrEmpty, result.Error);
    }


    [Theory]
    [InlineData("aa")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    public void Create_Should_ReturnsErrorWhenNameHasInvalidLength(string name)
    {
        // Arrange
        Guid type = Guid.NewGuid();
        decimal weight = 10.00M;
        string brand = "Brand";
        string model = "Model";
        string? notes = null;
        Guid user = Guid.NewGuid();

        // Act
        var result = Bike.Create(name, type, weight, brand, model, notes, user);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Bike.NameLengthIsInvalid(
                GlobalConstants.Bike.NameMinLength,
                GlobalConstants.Bike.NameMaxLength), 
            result.Error);
    }

    [Fact]
    public void Create_Should_ReturnsErrorWhenNaBikeTypeIsIsDefault()
    {
        // Arrange
        string name = "Name";
        Guid type = Guid.Empty;
        decimal weight = 10.00M;
        string brand = "Brand";
        string model = "Model";
        string? notes = null;
        Guid user = Guid.NewGuid();

        // Act
        var result = Bike.Create(name, type, weight, brand, model, notes, user);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Bike.BikeTypeIsInvalid, result.Error);
    }
    
    [Fact]
    public void Create_Should_ReturnsErrorWhenWeightIsOutOfRange()
    {
        // Arrange
        string name = "Name";
        Guid type = Guid.NewGuid();
        decimal weight = -0.01M;
        string brand = "Brand";
        string model = "Model";
        string? notes = null;
        Guid user = Guid.NewGuid();

        // Act
        var result = Bike.Create(name, type, weight, brand, model, notes, user);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Bike.WeightIsInvalid(
            GlobalConstants.Bike.WeightMinValue, 
            GlobalConstants.Bike.WeightMaxValue), result.Error);
    }
    
    [Fact]
    public void Create_Should_ReturnsErrorWhenBrandIsNullOrEmpty()
    {
        // Arrange
        string name = "Name";
        Guid type = Guid.NewGuid();
        decimal weight = 10.00M;
        string brand = "";
        string model = "Model";
        string? notes = null;
        Guid user = Guid.NewGuid();

        // Act
        var result = Bike.Create(name, type, weight, brand, model, notes, user);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Bike.BrandIsNullOrEmpty, result.Error);
    }


    [Theory]
    [InlineData("a")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    public void Create_Should_ReturnsErrorWhenBrandHasInvalidLength(string brand)
    {
        // Arrange
        string name = "Name";
        Guid type = Guid.NewGuid();
        decimal weight = 10.00M;
        string model = "Model";
        string? notes = null;
        Guid user = Guid.NewGuid();

        // Act
        var result = Bike.Create(name, type, weight, brand, model, notes, user);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Bike.BrandLengthIsInvalid(
                GlobalConstants.Bike.BrandMinLength,
                GlobalConstants.Bike.BrandMaxLength), 
            result.Error);
    }
    
    [Fact]
    public void Create_Should_ReturnsErrorWhenModelIsNullOrEmpty()
    {
        // Arrange
        string name = "Name";
        Guid type = Guid.NewGuid();
        decimal weight = 10.00M;
        string brand = "Brand";
        string model = "";
        string? notes = null;
        Guid user = Guid.NewGuid();

        // Act
        var result = Bike.Create(name, type, weight, brand, model, notes, user);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Bike.ModelIsNullOrEmpty, result.Error);
    }


    [Theory]
    [InlineData("a")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    public void Create_Should_ReturnsErrorWhenModelHasInvalidLength(string model)
    {
        // Arrange
        string name = "Name";
        Guid type = Guid.NewGuid();
        decimal weight = 10.00M;
        string brand = "Brand";
        string? notes = null;
        Guid user = Guid.NewGuid();

        // Act
        var result = Bike.Create(name, type, weight, brand, model, notes, user);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Bike.ModelLengthIsInvalid(
                GlobalConstants.Bike.ModelMinLength,
                GlobalConstants.Bike.ModelMaxLength), 
            result.Error);
    }
    
    [Fact]
    public void Create_Should_ReturnsErrorWhenNotesHasInvalidLength()
    {
        // Arrange
        string name = "Name";
        Guid type = Guid.NewGuid();
        decimal weight = 10.00M;
        string brand = "Brand";
        string model = "Model";
        string? notes = new string('n', 256);
        Guid user = Guid.NewGuid();

        // Act
        var result = Bike.Create(name, type, weight, brand, model, notes, user);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Bike.NotesLengthIsInvalid(
            GlobalConstants.Bike.NotesMaxLength), result.Error);
    }
    
    [Fact]
    public void Update_Should_ReturnErrorWhenUserDoesNotMatch()
    {
        // Arrange
        string name = "Name";
        Guid type = Guid.NewGuid();
        decimal weight = 10.00M;
        string brand = "Brand";
        string model = "Model";
        string? notes = null;
        Guid user = Guid.NewGuid();
        
        string newName = "Name";
        Guid newType = Guid.NewGuid();
        decimal newWeight = 10.00M;
        string newBrand = "Brand";
        string newModel = "Model";
        string? newNotes = null;
        Guid newUser = Guid.NewGuid();

        // Act
        var bike = Bike.Create(name, type, weight, brand, model, notes, user).Value;
        var update = bike.Update(newName, newType, newWeight, newBrand, newModel, newNotes, newUser);

        //Assert
        Assert.True(update.IsFailure);
        Assert.Equal(DomainErrors.UnauthorizedAccess("Bike.Update"), update.Error);
    }
    
    [Fact]
    public void Update_Should_ReturnErrorWhenUpdateDataIsInvalid()
    {
        // Arrange
        string name = "Name";
        Guid type = Guid.NewGuid();
        decimal weight = 10.00M;
        string brand = "Brand";
        string model = "Model";
        string? notes = null;
        Guid user = Guid.NewGuid();
        
        string newName = "";
        decimal newWeight = 10.00M;
        string newBrand = "Brand";
        string newModel = "Model";
        string? newNotes = null;

        // Act
        var bike = Bike.Create(name, type, weight, brand, model, notes, user).Value;
        var update = bike.Update(newName, type, newWeight, newBrand, newModel, newNotes, user);

        //Assert
        Assert.True(update.IsFailure);
        Assert.Equal(DomainErrors.Bike.NameIsNullOrEmpty, update.Error);
    }
    
    [Fact]
    public void Update_Should_ReturnSuccessWhenInputIsValid()
    {
        // Arrange
        string name = "Name";
        Guid type = Guid.NewGuid();
        decimal weight = 10.00M;
        string brand = "Brand";
        string model = "Model";
        string? notes = null;
        Guid user = Guid.NewGuid();
        
        string newName = "New Name";
        Guid newType = Guid.NewGuid();
        decimal newWeight = 20.00M;
        string newBrand = "New Brand";
        string newModel = "New Model";
        string? newNotes = "Notes";

        // Act
        var bike = Bike.Create(name, type, weight, brand, model, notes, user).Value;
        var update = bike.Update(newName, newType, newWeight, newBrand, newModel, newNotes, user);

        //Assert
        Assert.True(update.IsSuccess);
        Assert.False(update.IsFailure);
        Assert.Equal(newName, bike.Name);
        Assert.Equal(newType, bike.BikeTypeId);
        Assert.Equal(newWeight, bike.Weight);
        Assert.Equal(newBrand, bike.Brand);
        Assert.Equal(newModel, bike.Model);
        Assert.Equal(newNotes, bike.Notes);
        Assert.Equal(user, bike.UserId);
    }
}