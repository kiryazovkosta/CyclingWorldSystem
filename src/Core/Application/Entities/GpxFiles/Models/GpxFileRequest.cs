﻿using Microsoft.AspNetCore.Http;

namespace Application.Entities.GpxFiles.Models;

public sealed record GpxFileRequest(IEnumerable<IFormFile> Files);