namespace Application.Entities.GpxFiles.Commands.ParseGpxFile;

using Application.Abstractions.Messaging;
using Application.Entities.GpxFiles.Models;
using Application.Interfaces;
using Domain.Shared;
using Mapster;
using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Abstractions;

public class ParseGpxFileCommandHandler
	: ICommandHandler<ParseGpxFileCommand, GpxFileResponse>
{
	private readonly IGpxService gpxService;
	private readonly IGeoCoordinate geoCoordinateService;
	private readonly IWaypointRepository waypointRepository;
	private readonly IUnitOfWork unitOfWork;

	public ParseGpxFileCommandHandler(
		IGpxService gpxService, 
		IGeoCoordinate geoCoordinateService,
		IWaypointRepository waypointRepository, 
		IUnitOfWork unitOfWork)
	{
		this.gpxService = gpxService ?? throw new ArgumentNullException(nameof(gpxService));
		this.geoCoordinateService =
			geoCoordinateService ?? throw new ArgumentNullException(nameof(geoCoordinateService));
		this.waypointRepository = waypointRepository ?? throw new ArgumentNullException(nameof(waypointRepository));
		this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Result<GpxFileResponse>> Handle(
		ParseGpxFileCommand request, 
		CancellationToken cancellationToken)
	{
		var gpx =
			await this.gpxService.Get(
				DecodeFrom64(
					request.GpxFile));

		var gpxId = Guid.NewGuid();
		double? currentDistance = 0.0;
		double? distance = 0.0;
		decimal? positiveElevation = 0.0m;
		decimal? negativeElevation = 0.0m;

		var startTrkElement = gpx.Trk.Trkseg
			.Where(x => x.Time > DateTime.MinValue)
			.MinBy(x => x.Time);
		var startDateTime = DateTime.MinValue;
		if (startTrkElement is not null)
		{
			startDateTime = startTrkElement.Time;
		}

		var endDateTime = gpx.Trk.Trkseg.Max(x => x.Time);

		var waypoints = new HashSet<Waypoint>();

		for (var i = 0; i < gpx.Trk.Trkseg.Length; i+=5)
		{
			var current = gpx.Trk.Trkseg[i];
			if (i > 0)
			{
				var prev = gpx.Trk.Trkseg[i - 1];

				if (current.Elevation.HasValue && prev.Elevation.HasValue)
				{
					var offsetElevation = current.Elevation - prev.Elevation;
					if (offsetElevation < 0)
					{
						negativeElevation += Math.Abs(offsetElevation.Value);
					}
					else if (offsetElevation > 0)
					{
						positiveElevation += Math.Abs(offsetElevation.Value);
					}
				}

                currentDistance = await this.geoCoordinateService.GetDistance(prev.Longitude, prev.Latitude, current.Longitude, current.Latitude);
				distance += currentDistance;
			}

			var waypoint = current.Adapt<WaypointResponse>();
			waypoint.SetOrderNumber(i + 1);
			waypoint.SetSpeed(currentDistance * 3.6);
			var dbWaypoint = Waypoint.Create(
				null, waypoint.OrderNumber, waypoint.Latitude, waypoint.Longitude, waypoint.Elevation, 
				waypoint.Time, waypoint.Temperature, waypoint.HeartRate, waypoint.Power, waypoint.Speed, gpxId);
			if (dbWaypoint.IsSuccess)
			{
				waypoints.Add(dbWaypoint.Value);
			}
		}
		
		this.waypointRepository.AddRange(waypoints);
		await this.unitOfWork.SaveChangesAsync(cancellationToken);

		return new GpxFileResponse(
			gpxId,
			startDateTime,
			distance / 1000,
			negativeElevation,
			positiveElevation,
			endDateTime - startDateTime);

	}

	private static string DecodeFrom64(string encodedData)
    {
        byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
        string returnValue = System.Text.UTF8Encoding.UTF8.GetString(encodedDataAsBytes);
        return returnValue;

    }
}