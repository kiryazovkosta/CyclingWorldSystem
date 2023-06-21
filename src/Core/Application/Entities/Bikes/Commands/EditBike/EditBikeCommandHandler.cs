namespace Application.Entities.Bikes.Commands.EditBike;

using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class EditBikeCommandHandler
	: ICommandHandler<EditBikeCommand, Unit>
{
	private readonly IBikeRepository _bikeRepository;
	private readonly IUnitOfWork _unitOfWork;

	public EditBikeCommandHandler(IBikeRepository bikeRepository, IUnitOfWork unitOfWork)
	{
		_bikeRepository = bikeRepository ?? throw new ArgumentNullException(nameof(bikeRepository));
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Result<Unit>> Handle(EditBikeCommand request, CancellationToken cancellationToken)
	{
		Bike? bike = await this._bikeRepository.GetByIdAsync(request.Id, cancellationToken);
		if (bike is null)
		{
			return Result.Failure<Unit>(DomainErrors.Bike.BikeDoesNotExists(request.Id));
		}

		request.Adapt(bike);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return Unit.Value;
	}
}