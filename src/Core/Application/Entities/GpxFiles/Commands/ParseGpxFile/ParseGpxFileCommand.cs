namespace Application.Entities.GpxFiles.Commands.ParseGpxFile;

using Application.Abstractions.Messaging;
using Application.Entities.GpxFiles.Models;

public sealed record ParseGpxFileCommand(string GpxFile) : ICommand<GpxFileResponse>;