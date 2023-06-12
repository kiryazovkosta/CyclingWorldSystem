namespace Application.Entities.Bikes.Models;

using System.ComponentModel;

public sealed class CreateBikeRequest
{
	public string Brand { get; set; } = null!;

	/// <summary>
	/// The model of the bike as string. Maximum length of 50 symbols is alowed.
	/// </summary>
	public string Model { get; set; } = null!;
}