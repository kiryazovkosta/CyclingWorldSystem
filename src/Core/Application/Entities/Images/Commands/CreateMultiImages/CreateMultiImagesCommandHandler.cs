// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateMultiImagesCommandHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Images.Commands.CreateMultiImages;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Shared;
using Interfaces;

public class CreateMultiImagesCommandHandler
    : ICommandHandler<CreateMultiImagesCommand, List<string>>
{
    private readonly ICloudinaryService cloudinaryService;

    public CreateMultiImagesCommandHandler(ICloudinaryService cloudinaryService)
    {
        this.cloudinaryService = cloudinaryService ?? throw new ArgumentNullException(nameof(cloudinaryService));
    }

    public async Task<Result<List<string>>> Handle(
        CreateMultiImagesCommand request, 
        CancellationToken cancellationToken)
    {
        foreach (var file in request.Files)
        {
            bool isValid = cloudinaryService.IsFileValid(file);
            if (!isValid)
            {
                return Result.Failure<List<string>>(DomainErrors.Image.InvalidFileType);
            }
        }


        var urls = await cloudinaryService.UploadMultiAsync(request.Files);
        return urls;
    }
}