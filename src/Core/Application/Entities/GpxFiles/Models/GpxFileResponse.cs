using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.GpxFiles.Models;

public sealed record GpxFileResponse(
	DateTime StartDateTime,
	double? Distance,
	decimal? PositiveElevation,
	decimal? NegativeElevation,
	TimeSpan Duration,
	IEnumerable<WaypointResponse> Waypoints);
