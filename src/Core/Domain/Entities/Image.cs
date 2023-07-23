namespace Domain.Entities;

using Domain.Primitives;
using System;
using Shared;

public class Image : DeletableEntity
{
	private Image()
	{
	}

	private Image(string url, Activity? activity)
		: this()
	{
		Url = url;
		Activity = activity;
	}

	public string Url { get; set; } = null!;

	public Guid? ActivityId { get; set; }
	public Activity? Activity { get; set; }

	public static Result<Image> Create(string url, Activity activity)
	{
		var image = new Image(url, activity);
		return image;
	}
}