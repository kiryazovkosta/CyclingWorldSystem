namespace Application.Entities.Bikes.Commands.CreateBike;

using Application.Abstractions.Messaging;
using Domain.Repositories;
using Domain.Entities;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Errors;

public sealed class CreateBikeCommandHandler 
	: ICommandHandler<CreateBikeCommand, Guid>
{
	private readonly IBikeRepository _bikeRepository;
	private readonly ICurrentUserService _currentUserService;
	private readonly IUnitOfWork _unitOfWork;

	public CreateBikeCommandHandler(
		IBikeRepository bikeRepository, 
		ICurrentUserService currentUserService, 
		IUnitOfWork unitOfWork)
	{
		_bikeRepository = bikeRepository ?? throw new ArgumentNullException(nameof(bikeRepository));
		_currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Result<Guid>> Handle(CreateBikeCommand request, CancellationToken cancellationToken)
	{
		Guid userId = this._currentUserService.GetCurrentUserId();
		if (userId == Guid.Empty)
		{
			return Result.Failure<Guid>(
				DomainErrors.UnauthorizedAccess(
					nameof(CreateBikeCommand)));
		}

		var bikeResult = Bike.Create(
			request.Name,
			request.BikeTypeId,
			request.Weight,
			request.Brand, 
			request.Model,
			request.Notes,
			userId);
		if (bikeResult.IsFailure) 
		{
			return Result.Failure<Guid>(bikeResult.Error);
		}
		
		var bike = bikeResult.Value;
		_bikeRepository.Add(bike);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return bike.Id;
	}
}