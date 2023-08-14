namespace Domain.Entities;

using Common.Constants;
using Domain.Identity;
using Domain.Primitives;
using Errors;
using Shared;

public class Comment : DeletableEntity
{
	private Comment()
	{
	}

	private Comment(Guid activityId, Guid UserId, string content)
		: this() 
	{
		this.ActivityId = activityId;
		this.UserId = UserId;
		this.Content = content;
	}

	public string Content { get; set; } = null!;

	public Guid UserId { get; set; }
	public User User { get; set; } = null!;

	public Guid ActivityId { get; set; }
	public Activity Activity { get; set; } = null!;

	public static Result<Comment> Create(Guid activityId, Guid userId, string content)
	{
		if (string.IsNullOrWhiteSpace(content))
		{
			return Result.Failure<Comment>(DomainErrors.Comments.ContentIsEmptyOrNull);
		}

		if (content.Length < GlobalConstants.Comment.ContentMinLength 
			|| content.Length > GlobalConstants.Comment.ContentMaxLength)
		{
			return Result.Failure<Comment>(DomainErrors.Comments.ContentInvalidLength);
		}

		var comment = new Comment(activityId, userId, content);
		return comment; 
	}

	public Result Update(string content)
	{
		if (string.IsNullOrWhiteSpace(content))
		{
			return Result.Failure(DomainErrors.Comments.ContentIsEmptyOrNull);
		}

		if (content.Length is < GlobalConstants.Comment.ContentMinLength or > GlobalConstants.Comment.ContentMaxLength)
		{
			return Result.Failure(DomainErrors.Comments.ContentInvalidLength);
		}
		
		this.Content = content;
		return Result.Success();
	}
}