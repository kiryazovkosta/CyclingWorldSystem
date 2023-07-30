// ------------------------------------------------------------------------------------------------
//  <copyright file="CommentInputModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models.Comments;

public class CommentInputModel
{
    public Guid ActivityId { get; set; }
    public string Content { get; set; } = null!;
}