using Application.Abstractions.Messaging;
using Application.Entities.Bikes.Models;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Shared;
using Mapster;

namespace Application.Entities.Bikes.Queries.GetBikeById;

public class GetBikeByIdQueryHandler
	: IQueryHandler<GetBikeByIdQuery, BikeResponse>
{
	private readonly IBikeRepository _bikeRepository;

	public GetBikeByIdQueryHandler(IBikeRepository bikeRepository)
	{
		_bikeRepository = bikeRepository ?? throw new ArgumentNullException(nameof(bikeRepository));
	}

	public async Task<Result<BikeResponse>> Handle(GetBikeByIdQuery request, CancellationToken cancellationToken)
	{
		var bike = await _bikeRepository.GetByIdAsync(request.Id, cancellationToken);
		if (bike is null)
		{
			return Result.Failure<BikeResponse>(DomainErrors.Bike.BikeDoesNotExists(request.Id));
		}

		var response = bike.Adapt<BikeResponse>();
		return response;
	}
}
