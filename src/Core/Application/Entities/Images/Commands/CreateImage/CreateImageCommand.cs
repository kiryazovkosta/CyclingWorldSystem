namespace Application.Entities.Images.Commands.CreateImage;

using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

public sealed record CreateImageCommand(IFormFile File) : ICommand<string>;