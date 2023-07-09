namespace Application.Entities.GpxFiles.Commands.ParseGpxFile;

using Application.Abstractions.Messaging;
using Application.Entities.GpxFiles.Models;
using Application.Interfaces;
using Application.Services;
using Domain.Shared;
using Mapster;
using System;
using System.Threading;
using System.Threading.Tasks;

public class ParseGpxFileCommandHandler
	: ICommandHandler<ParseGpxFileCommand, GpxFileResponse>
{
	private readonly IGpxService gpxService;
	private readonly IGeoCoordinate geoCoordinateService;

	public ParseGpxFileCommandHandler(
		IGpxService gpxService, 
		IGeoCoordinate geoCoordinateService)
	{
		this.gpxService = gpxService ?? throw new ArgumentNullException(nameof(gpxService));
		this.geoCoordinateService = geoCoordinateService ?? throw new ArgumentNullException(nameof(geoCoordinateService));
	}

	public async Task<Result<GpxFileResponse>> Handle(
		ParseGpxFileCommand request, 
		CancellationToken cancellationToken)
	{
		var gpx = await this.gpxService.Get(request.Xml);

		double? currentDistance = 0.0;
		double? distance = 0.0;
		decimal? positiveElevation = 0.0m;
		decimal? negativeElevation = 0.0m;

		var startTrkElement = gpx.Trk.Trkseg.Where(x => x.Time > DateTime.MinValue).OrderBy(x => x.Time).FirstOrDefault();
		var startDateTime = DateTime.MinValue;
		if (startTrkElement is not null)
		{
			startDateTime = startTrkElement.Time;
		}

		var endDateTime = gpx.Trk.Trkseg.Max(x => x.Time);

		var waypoints = new HashSet<WaypointResponse>();

		for (int i = 0; i < gpx.Trk.Trkseg.Length; i++)
		{
			var current = gpx.Trk.Trkseg[i];
			if (i > 0)
			{
				var prev = gpx.Trk.Trkseg[i - 1];

				if (current.Elevation.HasValue && prev.Elevation.HasValue)
				{
					var offsetElevation = current.Elevation - prev.Elevation;
					if (offsetElevation.HasValue && offsetElevation < 0)
					{
						negativeElevation += Math.Abs(offsetElevation.Value);
					}
					else if (offsetElevation.HasValue && offsetElevation > 0)
					{
						positiveElevation += Math.Abs(offsetElevation.Value);
					}
				}

				currentDistance = await geoCoordinateService.GetDistance(current.Longitude, current.Latitude, prev.Longitude, prev.Latitude);
				distance += currentDistance;
			}

			var waypoint = current.Adapt<WaypointResponse>();
			waypoint.SetOrderNumber(i + 1);
			waypoint.SetSpeed(currentDistance * 3.6);
			waypoints.Add(waypoint);
		}

		return new GpxFileResponse(
			startDateTime,
			distance / 1000,
			negativeElevation,
			positiveElevation,
			endDateTime - startDateTime,
			waypoints);

	}
}
