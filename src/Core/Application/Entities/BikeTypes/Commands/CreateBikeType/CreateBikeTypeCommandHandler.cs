namespace Application.Entities.BikeTypes.Commands.CreateBikeType;

using Application.Abstractions.Messaging;
using Application.Entities.BikeTypes.Models;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using Mapster;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreateBikeTypeCommandHandler : ICommandHandler<CreateBikeTypeCommand, Guid>
{
	private readonly IBikeTypeRepository _bikeTypeRepository;
	private readonly IUnitOfWork _unitOfWork;

	public CreateBikeTypeCommandHandler(IBikeTypeRepository bikeTypeRepository, IUnitOfWork unitOfWork)
	{
		_bikeTypeRepository = bikeTypeRepository ?? throw new ArgumentNullException(nameof(bikeTypeRepository));
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Result<Guid>> Handle(
		CreateBikeTypeCommand request, 
		CancellationToken cancellationToken)
	{
		var typeExists = await this._bikeTypeRepository.ExistsAsync(request.Name, cancellationToken);

		var result = BikeType.Create(request.Name, typeExists);
		if (result.IsFailure)
		{
			return Result.Failure<Guid>(result.Error);
		}

		var bikeType = result.Value;
		_bikeTypeRepository.Add(bikeType);
		await this._unitOfWork.SaveChangesAsync(cancellationToken);
		return bikeType.Id;
	}
}
