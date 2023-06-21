namespace Application.Entities.Bikes.Commands.DeleteBike;

using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using MediatR;

public class DeleteBikeCommandHandler
	: ICommandHandler<DeleteBikeCommand, Unit>
{
	private readonly IBikeRepository _bikeRepository;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteBikeCommandHandler(IBikeRepository bikeRepository, IUnitOfWork unitOfWork)
	{
		_bikeRepository = bikeRepository ?? throw new ArgumentNullException(nameof(bikeRepository));
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Result<Unit>> Handle(DeleteBikeCommand request, CancellationToken cancellationToken)
	{
		var bike = await _bikeRepository.GetByIdAsync(request.Id, cancellationToken);
		if (bike is null)
		{
			return Result.Failure<Unit>(DomainErrors.Bike.BikeDoesNotExists(request.Id));
		}

		_bikeRepository.Delete(bike);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return Unit.Value;
	}
}