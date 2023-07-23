namespace Domain.Entities;

using Domain.Primitives;
using System;
using Common.Constants;
using Shared;

public class Waypoint : DeletableEntity
{
	private Waypoint()
	{
	}

	public Waypoint(Guid? activityId, 
		int orderIndex, 
		decimal latitude, 
		decimal longitude, 
		decimal? elevation, 
		DateTime time, 
		decimal? temperature, 
		int? heartRate, 
		int? power, 
		decimal? speed,
		Guid gpxId)
		: this()
	{
		ActivityId = activityId;
		OrderIndex = orderIndex;
		Latitude = latitude;
		Longitude = longitude;
		Elevation = elevation;
		Time = time;
		Temperature = temperature;
		HeartRate = heartRate;
		Power = power;
		Speed = speed;
		GpxId = gpxId;
	}

	public Guid? ActivityId { get; set; }
	public Activity? Activity { get; set; }
	public int OrderIndex { get; init; }
	public decimal Latitude { get; init; }
	public decimal Longitude { get; init; }
	public decimal? Elevation { get; init; }
	public DateTime Time { get; init; }
	public decimal? Temperature { get; init; }
	public int? HeartRate { get; init; }
	public int? Power { get; init; }
	public decimal? Speed { get; init; }
	public Guid GpxId { get; init; }

	public static Result<Waypoint> Create(
		Guid? activityId,
		int orderIndex,
		decimal latitude,
		decimal longitude,
		decimal? elevation,
		DateTime time,
		decimal? temperature,
		int? heartRate,
		int? power,
		decimal? speed,
		Guid gpxId)
	{
		var waypoint = new Waypoint(activityId, orderIndex, latitude, longitude,
			elevation, time, temperature, heartRate, power, speed, gpxId);
		return waypoint;
	}
}