namespace Application.Entities.GpxFiles.Models;

using Domain.Entities.GpxFile;
using Domain.Primitives;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class WaypointResponse
{
	public int OrderNumber { get; private set; }
	public decimal Latitude { get; init; }
	public decimal Longitude { get; init; }
	public decimal? Elevation { get; set; }
	public DateTime Time { get; init; }
	public decimal? Temperature { get; init; }
	public byte? HeartRate { get; init; }
	public decimal? Speed { get; private set; }
	public ushort? Power { get; init; }
	public byte? Cadance { get; init; }

	public void SetOrderNumber(int number)
		=> this.OrderNumber = number;

	public void SetSpeed(double? speed)
	{
		if (speed is not null
		    && speed.Value >= 0)
		{
			this.Speed = (decimal)speed;
		}
	}
}
