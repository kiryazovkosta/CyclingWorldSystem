﻿namespace Domain.Primitives;

public interface IAuditableEntity
{
	DateTime CreatedOn { get; set; }

	DateTime? ModifiedOn { get; set; }
}