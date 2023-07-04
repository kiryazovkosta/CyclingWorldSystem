namespace Application.Entities.BikeTypes.Queries.GetAllBikeTypes;

using Application.Abstractions.Messaging;
using Application.Entities.BikeTypes.Models;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class GetAllBikeTypesQueryHandler
	: IQueryHandler<GetAllBikeTypesQuery, IEnumerable<SimpleBikeTypeResponse>>
{
	private readonly IBikeTypeRepository _bikeTypeRepo;

	public GetAllBikeTypesQueryHandler(IBikeTypeRepository bikeTypeRepo)
	{
		this._bikeTypeRepo = bikeTypeRepo ?? throw new ArgumentNullException(nameof(bikeTypeRepo));
	}

	public async Task<Result<IEnumerable<SimpleBikeTypeResponse>>> Handle(
		GetAllBikeTypesQuery request, 
		CancellationToken cancellationToken)
	{
		var bikeTypes = await this._bikeTypeRepo.GetAllAsync(cancellationToken);
		if (bikeTypes is null) 
		{
			return Result.Failure<IEnumerable<SimpleBikeTypeResponse>>
				(DomainErrors.BikeType.BikeTypesCollectionIsNull);
		}

		var response = bikeTypes.Adapt<IEnumerable<SimpleBikeTypeResponse>>();
		return Result.Success(response);
	}
}