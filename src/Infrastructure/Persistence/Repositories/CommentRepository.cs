// ------------------------------------------------------------------------------------------------
//  <copyright file="CommentRepository.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Persistence.Repositories;

using Domain.Entities;
using Domain.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Add(Comment comment)
    {
        this._context.Set<Comment>().Add(comment);
    }
}