namespace Application.Entities.BikeTypes.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed record UpdateBikeTypeRequest(Guid Id, string Name);
