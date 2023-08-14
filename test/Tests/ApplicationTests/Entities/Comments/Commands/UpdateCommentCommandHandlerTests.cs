// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateCommentCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Comments.Commands;

using Application.Entities.Comments.Commands.UpdateComment;
using Application.Entities.Comments.Models;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Mapster;
using Moq;
using Persistence;

public class UpdateCommentCommandHandlerTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<ICommentRepository> commentRepository;
    private readonly Mock<IUnitOfWork> unitOfWork;


    public UpdateCommentCommandHandlerTests()
    {
        this._context = ApplicationDbContextTestFactory.Create();
        this.commentRepository = new Mock<ICommentRepository>();
        this.unitOfWork = new Mock<IUnitOfWork>();
    }

    public void Dispose()
    {
        this._context.Dispose();
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenCommentRepositoryIsNull()
    {
        //Arrange & Act
        Func<UpdateCommentCommandHandler> act = () => new UpdateCommentCommandHandler(
            null!,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'commentRepository')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenUnitOfWorkIsNull()
    {
        //Arrange & Act
        Func<UpdateCommentCommandHandler> act = () => new UpdateCommentCommandHandler(
            this.commentRepository.Object,
            null!);
    
        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'unitOfWork')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccessWhenCommandIsValid()
    {
        //Arrange
        var comment = this._context.Set<Comment>().First();
        var request = new UpdateCommentRequest(comment.Id, "New content");
        this.commentRepository.Setup(br => br.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<Comment?>(comment));
        var handler = new UpdateCommentCommandHandler(
            this.commentRepository.Object,
            this.unitOfWork.Object);
        var command = request.Adapt<UpdateCommentCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenCommentDoesNotExists()
    {
        //Arrange
        var commentId = Guid.NewGuid();
        var request = new UpdateCommentRequest(commentId, "New content");
        this.commentRepository.Setup(br => br.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<Comment?>(null));
        var handler = new UpdateCommentCommandHandler(
            this.commentRepository.Object,
            this.unitOfWork.Object);
        var command = request.Adapt<UpdateCommentCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.Comments.CommentDoesNotExists(commentId), result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenCommandIsInvalid()
    {
        //Arrange
        var comment = this._context.Set<Comment>().First();
        var request = new UpdateCommentRequest(comment.Id, "");
        this.commentRepository.Setup(br => br.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<Comment?>(comment));
        var handler = new UpdateCommentCommandHandler(
            this.commentRepository.Object,
            this.unitOfWork.Object);
        var command = request.Adapt<UpdateCommentCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.Comments.ContentIsEmptyOrNull, result.Error);
    }
}