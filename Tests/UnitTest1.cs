using Domain.Entities;
using Xunit;

namespace Tests;

public class UnitTest1
{
	[Fact]
	public void Test1()
	{
		// Arrange
		var id = Guid.NewGuid();
		var brand = "Brand";
		var model = "Model";

		// Act
		var bike = new Bike(id, brand, model);

		// Assert
		Assert.Equal(id, bike.Id);
		Assert.Equal(brand, bike.Brand);
		Assert.Equal(model, bike.Model);
	}
}