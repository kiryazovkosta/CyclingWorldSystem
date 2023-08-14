// ------------------------------------------------------------------------------------------------
//  <copyright file="GetCommentByIdQueryHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Comments.Queries;

using Application.Entities.Comments.Queries.GetCommentById;
using Domain.Entities;
using Domain.Repositories;
using Persistence.Repositories;

public class GetCommentByIdQueryHandlerTests : TestApplicationContext
{
    private readonly ICommentRepository commentRepository;

    public GetCommentByIdQueryHandlerTests()
    {
        this.commentRepository = new CommentRepository(this.Context);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<GetCommentByIdQueryHandler> act = () => new GetCommentByIdQueryHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'commentRepository')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnsRecord()
    {
        //Arrange
        var comment = this.Context.Set<Comment>().First();
        var handler =  new GetCommentByIdQueryHandler(this.commentRepository);
        var query = new GetCommentByIdQuery(comment.Id);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        var commentResponse = result.Value;
        Assert.Equal(comment.Id, commentResponse.Id);
        Assert.Equal(comment.Content, commentResponse.Content);
        Assert.Equal(comment.ActivityId, commentResponse.ActivityId);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnsFailure()
    {
        //Arrange
        var handler =  new GetCommentByIdQueryHandler(this.commentRepository);
        var query = new GetCommentByIdQuery(Guid.NewGuid());
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        //Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
    }
}