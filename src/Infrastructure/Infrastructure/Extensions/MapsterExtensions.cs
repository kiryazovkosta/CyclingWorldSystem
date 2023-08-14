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

using Application.Entities.Activities.Models;
using Domain.Entities;

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

		TypeAdapterConfig<Activity, SimplyActivityResponse>
			.NewConfig()
			.Map(dest => dest.FirstPicture, 
				src => src.Images.FirstOrDefault()!.Url,
				srcOpt => srcOpt.Images.Any() && srcOpt.Images.FirstOrDefault() != null)
			.Map(dest => dest.UserName, src => src.User.FullName)
			.Map(dest => dest.Avatar, src => src.User.ImageUrl);

		TypeAdapterConfig<Activity, ActivityResponse>
			.NewConfig()
			.Map(dest => dest.Images,
				src => src.Images.Select(i => i.Url),
				srcOpt => srcOpt.Images.Any())
			.Map(dest => dest.UserName, src => src.User.FullName)
			.Map(dest => dest.Avatar, src => src.User.ImageUrl)
			.Map(dest => dest.Bike, src => src.Bike.Name)
			.Map(dest => dest.LikeCount, src => src.Likes.Count())
			.Ignore(dest => dest.IsLikedByMe)
			.Ignore(dest => dest.Comments);


		TypeAdapterConfig.GlobalSettings.Scan(
			Application.AssemblyReference.Assembly,
			Domain.AssemblyReference.Assembly);
	}
}
