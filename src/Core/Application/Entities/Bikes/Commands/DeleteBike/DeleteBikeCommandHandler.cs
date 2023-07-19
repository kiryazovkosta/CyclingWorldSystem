namespace Application.Entities.Bikes.Commands.DeleteBike;

using Abstractions.Messaging;
using Domain.Repositories;
using Domain.Errors;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using MediatR;

public class DeleteBikeCommandHandler
	: ICommandHandler<DeleteBikeCommand>
{
	private readonly IBikeRepository _bikeRepository;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteBikeCommandHandler(IBikeRepository bikeRepository, IUnitOfWork unitOfWork)
	{
		_bikeRepository = bikeRepository ?? throw new ArgumentNullException(nameof(bikeRepository));
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Result> Handle(
		DeleteBikeCommand request, 
		CancellationToken cancellationToken)
	{
		var deleteResult = await this._bikeRepository.DeleteAsync(request.Id, cancellationToken);
		if (!deleteResult)
		{
			return Result.Failure<Unit>(DomainErrors.Bike.BikeDoesNotExists(request.Id));
		}
		
		await this._unitOfWork.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}
}