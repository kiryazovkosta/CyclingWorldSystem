// ------------------------------------------------------------------------------------------------
//  <copyright file="GetBikeTypeByIdQueryHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.BikeTypes.Queries.GetBikeTypeById;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Mapster;
using Models;

public class GetBikeTypeByIdQueryHandler
    : IQueryHandler<GetBikeTypeByIdQuery, SimpleBikeTypeResponse>
{
    private readonly IBikeTypeRepository bikeTypeRepository;

    public GetBikeTypeByIdQueryHandler(IBikeTypeRepository bikeTypeRepository)
    {
        this.bikeTypeRepository = bikeTypeRepository ?? throw new ArgumentNullException(nameof(bikeTypeRepository));
    }

    public async Task<Result<SimpleBikeTypeResponse>> Handle(
        GetBikeTypeByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var bikeType = await this.bikeTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (bikeType is null)
        {
            return Result.Failure<SimpleBikeTypeResponse>(DomainErrors.BikeType.BikeTypeDoesNotExists);
        }

        var response = bikeType.Adapt<SimpleBikeTypeResponse>();
        return response;
    }
}