// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateMultiImagesCommand.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Images.Commands.CreateMultiImages;

using Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

public sealed record CreateMultiImagesCommand(List<IFormFile> Files) : ICommand<List<string>>;