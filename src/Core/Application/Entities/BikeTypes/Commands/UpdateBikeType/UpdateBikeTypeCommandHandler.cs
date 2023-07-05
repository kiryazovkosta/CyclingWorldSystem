namespace Application.Entities.BikeTypes.Commands.UpdateBikeType;

using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UpdateBikeTypeCommandHandler : ICommandHandler<UpdateBikeTypeCommand>
{
	private readonly IBikeTypeRepository bikeTypeRepository;
	private readonly IUnitOfWork unitOfWork;

	public UpdateBikeTypeCommandHandler(
		IBikeTypeRepository bikeTypeRepository, 
		IUnitOfWork unitOfWork)
	{
		this.bikeTypeRepository = bikeTypeRepository ?? throw new ArgumentNullException(nameof(bikeTypeRepository));
		this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Result> Handle(UpdateBikeTypeCommand request, CancellationToken cancellationToken)
	{
		var bikeType = await this.bikeTypeRepository.GetByIdAsync(request.Id, cancellationToken);
		if (bikeType is null) 
		{
			return Result.Failure(DomainErrors.BikeType.BikeTypeDoesNotExists);
		}

		var exists = await this.bikeTypeRepository.ExistsAsync(request.Name, cancellationToken);
		if (exists)
		{
			return Result.Failure(DomainErrors.BikeType.BikeTypeNameExists(request.Name));
		}

		var result = bikeType.Update(request.Name);
		if (result.IsFailure)
		{
			return Result.Failure(result.Error);
		}

		this.bikeTypeRepository.Update(bikeType);
		await this.unitOfWork.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}
}
