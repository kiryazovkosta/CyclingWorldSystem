// ------------------------------------------------------------------------------------------------
//  <copyright file="DeleteBikeTypeCommandHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.BikeTypes.Commands.DeleteBikeType;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Domain.Shared;

public class DeleteBikeTypeCommandHandler
    : ICommandHandler<DeleteBikeTypeCommand>
{
    private readonly IBikeTypeRepository _bikeTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBikeTypeCommandHandler(IBikeTypeRepository bikeTypeRepository, IUnitOfWork unitOfWork)
    {
        this._bikeTypeRepository = bikeTypeRepository ?? throw new ArgumentNullException(nameof(bikeTypeRepository));
        this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result> Handle(
        DeleteBikeTypeCommand request, 
        CancellationToken cancellationToken)
    {
        var result = await _bikeTypeRepository.DeleteAsync(request.Id, cancellationToken);
        if (!result)
        {
            return Result.Failure(
                DomainErrors.DeleteOperationFailed(request.Id, "DeleteBikeCommandHandler"));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}