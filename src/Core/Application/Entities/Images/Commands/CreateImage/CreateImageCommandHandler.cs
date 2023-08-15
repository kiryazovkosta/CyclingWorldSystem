namespace Application.Entities.Images.Commands.CreateImage;

using Application.Abstractions.Messaging;
using Application.Interfaces;
using Domain.Errors;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class CreateImageCommandHandler
    : ICommandHandler<CreateImageCommand, string>
{
    private readonly ICloudinaryService cloudinaryService;

    public CreateImageCommandHandler(ICloudinaryService cloudinaryService)
    {
        this.cloudinaryService = cloudinaryService ?? throw new ArgumentNullException(nameof(cloudinaryService));
    }

    public async Task<Result<string>> Handle(
        CreateImageCommand request, 
        CancellationToken cancellationToken)
    {
        bool isValid = cloudinaryService.IsFileValid(request.File);
        if (!isValid)
        {
            return Result.Failure<string>(DomainErrors.Image.InvalidFileType);
        }

        var url = await cloudinaryService.UploadAsync(request.File);
        return Result.Success(url);
    }
}
