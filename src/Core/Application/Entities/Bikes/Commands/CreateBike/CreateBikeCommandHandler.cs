namespace Application.Entities.Bikes.Commands.CreateBike;

using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

public sealed class CreateBikeCommandHandler 
	: ICommandHandler<CreateBikeCommand, Bike>
{
	private readonly IBikeRepository _bikeRepository;
	private readonly IUnitOfWork _unitOfWork;

	public CreateBikeCommandHandler(
		IBikeRepository bikeRepository,
		IUnitOfWork unitOfWork)
	{
		_bikeRepository = bikeRepository ?? throw new ArgumentNullException(nameof(bikeRepository));
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Result<Bike>> Handle(CreateBikeCommand request, CancellationToken cancellationToken)
	{
		var bikeResult = Bike.Create(request.Brand, request.Model);
		var bike = bikeResult.Value;
		_bikeRepository.Add(bike);
		await _unitOfWork.SaveChangesAsync();
		return bike;
	}
}