namespace Application.Entities.Bikes.Models;

using System;

public sealed record CreateBikeRequest(string Brand, string Model);