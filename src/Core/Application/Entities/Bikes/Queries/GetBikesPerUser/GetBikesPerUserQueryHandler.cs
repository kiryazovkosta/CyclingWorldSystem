namespace Application.Entities.Bikes.Queries.GetBikes;

using Application.Abstractions.Messaging;
using Application.Entities.Bikes.Models;
using Domain.Repositories;
using Domain.Shared;
using Mapster;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GetBikesPerUserQueryHandler
	: IQueryHandler<GetBikesPerUserQuery, List<BikeResponse>>
{
	private readonly IBikeRepository _bikeRepository;

	public GetBikesPerUserQueryHandler(IBikeRepository bikeRepository)
	{
		_bikeRepository = bikeRepository ?? throw new ArgumentNullException(nameof(bikeRepository));
	}

	public async Task<Result<List<BikeResponse>>> Handle(GetBikesPerUserQuery request, CancellationToken cancellationToken)
	{
		var bikes = await _bikeRepository.GetAllByUserAsync(request.UserId, cancellationToken);
		var response = bikes.Adapt<List<BikeResponse>>();
		return response;
	}
}