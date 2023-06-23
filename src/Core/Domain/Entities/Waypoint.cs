namespace Domain.Entities;

using Domain.Primitives;
using System;

public class Waypoint : DeletableEntity
{
	public Guid ActivityId { get; set; }
	public Activity Activity { get; set; } = null!;

	public int OrderIndex { get; set; }

	public decimal Latitude { get; set; }

	public decimal Longitude { get; set; }

	public int Elevation { get; set; }

	public DateTime Time { get; set; }

	public int Temperature { get; set; }

	public int? HeartRate { get; set; }

	public int? Power { get; set; }

	public decimal Speed { get; set; }
}