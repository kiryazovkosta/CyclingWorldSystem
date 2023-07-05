namespace Application.Entities.Bikes.Commands.UpdateBike;

using Application.Abstractions.Messaging;
using Domain.Repositories;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using System;
using Application.Entities.Bikes.Commands.CreateBike;

public class UpdateBikeCommandHandler
	: ICommandHandler<UpdateBikeCommand>
{
	private readonly IBikeRepository _bikeRepository;
	private readonly ICurrentUserService _currentUserService;
	private readonly IUnitOfWork _unitOfWork;

	public UpdateBikeCommandHandler(
		IBikeRepository bikeRepository, 
		ICurrentUserService currentUserService, 
		IUnitOfWork unitOfWork)
	{
		_bikeRepository = bikeRepository ?? throw new ArgumentNullException(nameof(bikeRepository));
		_currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Result> Handle(
		UpdateBikeCommand request, 
		CancellationToken cancellationToken)
	{
		Bike? bike = await this._bikeRepository.GetByIdAsync(request.Id, cancellationToken);
		if (bike is null)
		{
			return Result.Failure(DomainErrors.Bike.BikeDoesNotExists(request.Id));
		}

		Guid userId = this._currentUserService.GetCurrentUserId();
		if (userId == Guid.Empty || userId != bike.UserId)
		{
			return Result.Failure<Bike>(
				DomainErrors.UnauthorizedAccess(nameof(UpdateBikeCommand)));
		}

		var result = bike.Update(
			request.Name, 
			request.BikeTypeId, 
			request.Weight, 
			request.Brand, 
			request.Model, 
			request.Notes,
			userId);
		if (result.IsFailure)
		{
			return Result.Failure(result.Error);
		}

		this._bikeRepository.Update(bike);
		await this._unitOfWork.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}
}