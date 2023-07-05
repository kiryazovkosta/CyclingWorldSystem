using Domain.Entities;
using Xunit;

namespace Tests;

public class UnitTest1
{
	[Fact]
	public void Test1()
	{
		// Arrange
		var name = "Name";
		var bikeTypeId = Guid.NewGuid();
		decimal weigth = 11.60M;
		var brand = "Brand";
		var model = "Model";
		string? notes = "My notes";
		var userId = Guid.NewGuid();

		// Act
		var bike = Bike.Create(name, bikeTypeId, weigth, brand, model, notes, userId).Value;

		// Assert
		Assert.Equal(name, bike.Name);
		Assert.Equal(bikeTypeId, bike.BikeTypeId);
		Assert.Equal(weigth, bike.Weight);
		Assert.Equal(brand, bike.Brand);
		Assert.Equal(model, bike.Model);
		Assert.Equal(notes, bike.Notes);
		Assert.Equal(userId, bike.UserId);
	}
}