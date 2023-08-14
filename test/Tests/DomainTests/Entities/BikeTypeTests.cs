namespace Tests.DomainTests.Entities;

using Common.Constants;
using Domain.Entities;
using Domain.Errors;

public class BikeTypeTests
{

    [Fact]
    public void Constructor_Should_CreateValidBikeType()
    {
        // Arrange
        string name = "Name";

        // Act
        var result = BikeType.Create(name, false);
        var bikeType = result.Value;

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(name, bikeType.Name);
        Assert.Empty(bikeType.Bikes);
    }

    [Fact]
    public void Create_Should_ReturnsErrorWhenNameIsNullOrEmpty()
    {
        // Arrange
        string name = "";

        // Act
        var result = BikeType.Create(name, false);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.BikeType.BikeTypeNameIsNull, result.Error);
    }

    [Fact]
    public void Create_Should_ReturnsErrorWhenNameLengthIsSmaller()
    {
        // Arrange
        string name = "a";

        // Act
        var result = BikeType.Create(name, false);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.BikeType.BikeTypeNameInvalidLength(
                    GlobalConstants.BikeType.NameMinLength,
                    GlobalConstants.BikeType.NameMaxLength), result.Error);
    }

    [Fact]
    public void Create_Should_ReturnsErrorWhenNameLengthIsGreater()
    {
        // Arrange
        string name = new string('a', 51); ;

        // Act
        var result = BikeType.Create(name, false);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.BikeType.BikeTypeNameInvalidLength(
                    GlobalConstants.BikeType.NameMinLength,
                    GlobalConstants.BikeType.NameMaxLength), result.Error);
    }

    [Fact]
    public void Create_Should_ReturnsErrorWhenNameExists()
    {
        // Arrange
        string name = new string("TestName");

        // Act
        var result = BikeType.Create(name, true);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.BikeType.BikeTypeNameExists(name), result.Error);
    }

    [Fact]
    public void Update_Should_ReturnsSuccessWhenNameIsValid()
    {
        // Arrange
        var bikeType = BikeType.Create("Name", false).Value;
        string name = "NewName";

        // Act
        var result = bikeType.Update(name);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal("NewName", bikeType.Name);
    }


    [Fact]
    public void Update_Should_ReturnsErrorWhenNameIsEmptyNullOrWhiteSpace()
    {
        // Arrange
        var bikeType = BikeType.Create("Name", false).Value;
        string name = "";

        // Act
        var result = bikeType.Update(name);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.BikeType.BikeTypeNameIsNull, result.Error);
    }

    [Fact]
    public void Update_Should_ReturnsErrorWhenNameLengthIsSmaller()
    {
        // Arrange
        var bikeType = BikeType.Create("Name", false).Value;
        string name = "a";

        // Act
        var result = bikeType.Update(name);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.BikeType.BikeTypeNameInvalidLength(
                    GlobalConstants.BikeType.NameMinLength,
                    GlobalConstants.BikeType.NameMaxLength), result.Error);
    }


    [Fact]
    public void Update_Should_ReturnsErrorWhenNameLengthIsGreater()
    {
        // Arrange
        var bikeType = BikeType.Create("Name", false).Value;
        string name = new string('a', GlobalConstants.BikeType.NameMaxLength + 1);

        // Act
        var result = bikeType.Update(name);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.BikeType.BikeTypeNameInvalidLength(
                    GlobalConstants.BikeType.NameMinLength,
                    GlobalConstants.BikeType.NameMaxLength), result.Error);
    }


}
