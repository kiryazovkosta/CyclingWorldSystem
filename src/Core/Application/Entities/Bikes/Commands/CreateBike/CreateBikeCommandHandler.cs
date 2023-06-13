namespace Application.Entities.Bikes.Commands.CreateBike;

using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Repositories.Abstractions;
using System;
using System.Threading.Tasks;

public sealed class CreateBikeCommandHandler 
	: ICommandHandler<CreateBikeCommand, Guid>
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

	public async Task<Guid> Handle(CreateBikeCommand request, CancellationToken cancellationToken)
	{
		var bike = Bike.Create(request.Brand, request.Model);
		this._bikeRepository.Add(bike);
		await this._unitOfWork.SaveChangesAsync(cancellationToken);
		return bike.Id;
	}
}