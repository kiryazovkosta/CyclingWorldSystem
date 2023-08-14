// ------------------------------------------------------------------------------------------------
//  <copyright file="DeleteCommentCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Comments.Commands;

using Application.Entities.Comments.Commands.DeleteComment;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Moq;
using Persistence;

public class DeleteCommentCommandHandlerTests : IDisposable
    {
    private readonly ApplicationDbContext _context;
    private readonly Mock<ICommentRepository> commentRepository;
    private readonly Mock<IUnitOfWork> unitOfWork;


    public DeleteCommentCommandHandlerTests()
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
        Func<DeleteCommentCommandHandler> act = () => new DeleteCommentCommandHandler(
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
        Func<DeleteCommentCommandHandler> act = () => new DeleteCommentCommandHandler(
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
        var command = new DeleteCommentCommand(comment.Id);
        this.commentRepository.Setup(br => br.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));
        var handler = new DeleteCommentCommandHandler(
            this.commentRepository.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccessWhenCommentDoesNotExists()
    {
        //Arrange
        var comment = this._context.Set<Comment>().First();
        this.commentRepository.Setup(br => br.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(false));
        var handler = new DeleteCommentCommandHandler(
            this.commentRepository.Object,
            this.unitOfWork.Object);
        var command = new DeleteCommentCommand(comment.Id);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.Comments.CommentDoesNotExists(command.Id), result.Error);
    }

}