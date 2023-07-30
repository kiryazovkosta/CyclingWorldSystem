// ------------------------------------------------------------------------------------------------
//  <copyright file="CommentViewModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models.Comments;

public class CommentViewModel
{
    public Guid Id { get; init; }
    public string UserName { get; init; } = null!;
    public DateTime CreatedOn { get; init; }
    public string Content { get; init; } = null!;
    public bool IsMine { get; init; }
}