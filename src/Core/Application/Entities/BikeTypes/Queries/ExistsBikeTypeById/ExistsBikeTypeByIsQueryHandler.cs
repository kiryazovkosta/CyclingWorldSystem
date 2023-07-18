namespace Application.Entities.BikeTypes.Queries.ExistsBikeTypeById;

using Application.Abstractions.Messaging;
using Domain.Repositories;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class ExistsBikeTypeByIsQueryHandler
    : IQueryHandler<ExistsBikeTypeByIsQuery, bool>
{
    private readonly IBikeTypeRepository bikeTypeRepository;

    public ExistsBikeTypeByIsQueryHandler(IBikeTypeRepository bikeTypeRepository)
    {
        this.bikeTypeRepository = bikeTypeRepository ?? throw new ArgumentNullException(nameof(bikeTypeRepository));
    }

    public async Task<Result<bool>> Handle(ExistsBikeTypeByIsQuery request, CancellationToken cancellationToken)
    {
        var exists = await this.bikeTypeRepository.ExistsByIdAsync(request.Id, cancellationToken);
        return exists;
    }
}
