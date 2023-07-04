namespace Application.Entities.Bikes.Queries.GetBikes;

using Application.Abstractions.Messaging;
using Application.Entities.Bikes.Models;
using Domain.Repositories;
using Domain.Shared;
using Mapster;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GetBikesQueryHandler
	: IQueryHandler<GetBikesQuery, List<SimpleBikeResponse>>
{
	private readonly IBikeRepository _bikeRepository;

	public GetBikesQueryHandler(IBikeRepository bikeRepository)
	{
		_bikeRepository = bikeRepository ?? throw new ArgumentNullException(nameof(bikeRepository));
	}

	public async Task<Result<List<SimpleBikeResponse>>> Handle(GetBikesQuery request, CancellationToken cancellationToken)
	{
		var bikes = await _bikeRepository.GetAllAsync(cancellationToken);
		var response = bikes.Adapt<List<SimpleBikeResponse>>();
		return response;
	}
}