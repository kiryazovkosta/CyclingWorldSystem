namespace Domain.Entities;

using Domain.Primitives;
using System;

public class Image : DeletableEntity
{
	public string Url { get; set; } = null!;

	public Guid? ActivityId { get; set; }
	public Activity? Activity { get; set; }
}