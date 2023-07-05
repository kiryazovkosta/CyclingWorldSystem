namespace Application.Entities.BikeTypes.Commands.DeleteBikeType;

using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using System;
using System.Threading.Tasks;

public class DeleteBikeCommandHandler : ICommandHandler<DeleteBikeTypeCommand>
{
	private readonly IBikeTypeRepository _productTypeRepository;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteBikeCommandHandler(
		IBikeTypeRepository productTypeRepository, 
		IUnitOfWork unitOfWork)
	{
		_productTypeRepository = productTypeRepository ?? throw new ArgumentNullException(nameof(productTypeRepository));
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Result> Handle(DeleteBikeTypeCommand request, CancellationToken cancellationToken)
	{
		var result = await _productTypeRepository.DeleteAsync(request.Id, cancellationToken);
		if (!result)
		{
			return Result.Failure(DomainErrors.DeleteOperationFailed(request.Id, "DeleteBikeCommandHandler"));
		}

		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}
}