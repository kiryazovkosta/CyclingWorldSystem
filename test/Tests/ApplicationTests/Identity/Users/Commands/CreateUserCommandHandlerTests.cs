// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateUserCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Users.Commands;

using Application.Identity.Users.Commands.CreateUser;
using Application.Interfaces;
using Domain.Identity;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Moq;
using Persistence;

public class CreateUserCommandHandlerTests : IDisposable
{
    private readonly ApplicationDbContext context;
    private readonly MockUserManager userManager;
    private readonly Mock<ICloudinaryService> cloudinaryService;
    private readonly Mock<IEmailSender> emailSender;
    private readonly Mock<IUnitOfWork> unitOfWork;

    public CreateUserCommandHandlerTests()
    {
        this.context = ApplicationDbContextTestFactory.Create();
        this.userManager = new MockUserManager();
        this.cloudinaryService = new Mock<ICloudinaryService>();
        this.emailSender = new Mock<IEmailSender>();
        this.unitOfWork = new Mock<IUnitOfWork>();
    }
    
    public void Dispose()
    {
        this.context.Dispose();
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<CreateUserCommandHandler> act = () => new CreateUserCommandHandler(
            null!,
            this.cloudinaryService.Object,
            this.emailSender.Object,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'userManager')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenIActivityLikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<CreateUserCommandHandler> act = () => new CreateUserCommandHandler(
            this.userManager.Object,
            null!,
            this.emailSender.Object,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'cloudinaryService')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenUserManagerIsNull()
    {
        //Arrange & Act
        Func<CreateUserCommandHandler> act = () => new CreateUserCommandHandler(
            this.userManager.Object,
            this.cloudinaryService.Object,
            null!,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'emailSender')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenUnitOfWorkIsNull()
    {
        //Arrange & Act
        Func<CreateUserCommandHandler> act = () => new CreateUserCommandHandler(
            this.userManager.Object,
            this.cloudinaryService.Object,
            this.emailSender.Object,
            null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'unitOfWork')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenCreateIsNotSuccess()
    {
        // Arrange
        this.userManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Failed(new IdentityError[]
                { new() { Code = "Code", Description = "Description" } })));
        var handler = new CreateUserCommandHandler(
            this.userManager.Object,
            this.cloudinaryService.Object,
            this.emailSender.Object,
            this.unitOfWork.Object);
        var command = new CreateUserCommand("UserName", "Email@example.com", "P@ssw0rd", "P@ssw0rd",
            "FirstName", "MiddleName", "LastName", null);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenCreateIsSuccessBytFindNot()
    {
        // Arrange
        this.userManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Success));
        this.userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(null));
        var handler = new CreateUserCommandHandler(
            this.userManager.Object,
            this.cloudinaryService.Object,
            this.emailSender.Object,
            this.unitOfWork.Object);
        var command = new CreateUserCommand("UserName", "Email@example.com", "P@ssw0rd", "P@ssw0rd",
            "FirstName", "MiddleName", "LastName", null);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.NullValue, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccessOnValidInput()
    {
        // Arrange
        var userId = Guid.NewGuid();
        this.userManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Success));
        this.userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User() { Id = userId }));
        this.userManager.Setup(um => um.GetUserIdAsync(It.IsAny<User>()))
            .Returns(Task.FromResult<string>("12345678"));
        this.userManager.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
            .Returns(Task.FromResult<string>("a1s2d3f4"));
        var handler = new CreateUserCommandHandler(
            this.userManager.Object,
            this.cloudinaryService.Object,
            this.emailSender.Object,
            this.unitOfWork.Object);
        var command = new CreateUserCommand("UserName", "Email@example.com", "P@ssw0rd", "P@ssw0rd",
            "FirstName", "MiddleName", "LastName", null);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(userId, result.Value);
    }
}