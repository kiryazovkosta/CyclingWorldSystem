namespace Domain.Entities;

using System;

public class Image
{
	public string Url { get; set; } = null!;

	public Guid? ActivityId { get; set; }
	public Activity? Activity { get; set; }
}