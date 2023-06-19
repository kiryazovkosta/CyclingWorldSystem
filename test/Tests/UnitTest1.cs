using Domain.Entities;
using Xunit;

namespace Tests;

public class UnitTest1
{
	[Fact]
	public void Test1()
	{
		// Arrange
		var brand = "Brand";
		var model = "Model";

		// Act
		var bike = Bike.Create(brand, model).Value;

		// Assert
		Assert.Equal(brand, bike.Brand);
		Assert.Equal(model, bike.Model);
	}
}