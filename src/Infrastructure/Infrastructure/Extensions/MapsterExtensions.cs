using Application.Entities.GpxFiles.Models;
using Application.Identity.Users.Commands.CreateUser;
using Domain.Entities.GpxFile;
using Domain.Identity;
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
				srcOpt => srcOpt.Extensions != null)
			.Map(dest => dest.Cadance, 
				src => src.Extensions.TrackPointExtension.Cadance,
				opt => opt.Extensions != null && opt.Extensions.TrackPointExtension != null)
			.Map(dest => dest.HeartRate, 
				src => src.Extensions.TrackPointExtension.HeartRate,
				opt => opt.Extensions != null && opt.Extensions.TrackPointExtension != null)
			.Map(dest => dest.Temperature, 
				src => src.Extensions.TrackPointExtension.Temperature,
				opt => opt.Extensions != null && opt.Extensions.TrackPointExtension != null);

		TypeAdapterConfig<CreateUserCommand, User>
			.NewConfig()
			.Ignore(dest => dest.ImageUrl);

        TypeAdapterConfig.GlobalSettings.Scan(
			Application.AssemblyReference.Assembly,
			Domain.AssemblyReference.Assembly);
	}
}
