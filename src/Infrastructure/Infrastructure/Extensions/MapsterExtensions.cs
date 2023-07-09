using Application.Entities.GpxFiles.Models;
using Domain.Entities.GpxFile;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions;

public static class MapsterExtensions
{
	public static void RegisterMapsterConfiguration(this IServiceCollection services)
	{
		TypeAdapterConfig<GpxTrkTrkpt, WaypointResponse>
			.NewConfig()
			.Map(dest => dest.Power, 
				src => src.Extensions.Power, 
				srcOpt => srcOpt.Extensions.Power.HasValue)
			.Map(dest => dest.Cadance, 
				src => src.Extensions.TrackPointExtension.Cadance,
				opt => opt.Extensions.TrackPointExtension.Cadance.HasValue)
			.Map(dest => dest.HeartRate, src => src.Extensions.TrackPointExtension.HeartRate)
			.Map(dest => dest.Temperature, src => src.Extensions.TrackPointExtension.Temperature);
			
		TypeAdapterConfig.GlobalSettings.Scan(
			Application.AssemblyReference.Assembly,
			Domain.AssemblyReference.Assembly);
	}
}
