namespace Application.Entities.BikeTypes.Commands.DeleteBikeType;

using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using System;
using System.Threading.Tasks;
using Bikes.Commands.DeleteBike;

public class DeleteBikeCommandHandler : ICommandHandler<DeleteBikeCommand>
{
	private readonly IBikeRepository _bikeRepository;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteBikeCommandHandler(
		IBikeRepository bikeRepository, 
		IUnitOfWork unitOfWork)
	{
		_bikeRepository = bikeRepository ?? throw new ArgumentNullException(nameof(bikeRepository));
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Result> Handle(DeleteBikeCommand request, CancellationToken cancellationToken)
	{
		var result = await _bikeRepository.DeleteAsync(request.Id, cancellationToken);
		if (!result)
		{
			return Result.Failure(DomainErrors.DeleteOperationFailed(request.Id, "DeleteBikeCommandHandler"));
		}

		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}
}