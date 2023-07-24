namespace Application.Identity.Users.Commands.DeleteUser;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Identity;
using Domain.Shared;
using Interfaces;
using Microsoft.AspNetCore.Identity;

public class DeleteUserCommandHandler
    : ICommandHandler<DeleteUserCommand>
{
    private readonly UserManager<User> userManager;
    private readonly ICurrentUserService currentUserService;

    public DeleteUserCommandHandler(
        UserManager<User> userManager, 
        ICurrentUserService currentUserService)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
        {
            return Result.Failure(DomainErrors.DeleteOperationFailed(request.Id, nameof(DeleteUserCommand)));
        }

        var currentIUserId = this.currentUserService.GetCurrentUserId();
        if (request.Id == currentIUserId)
        {
            return Result.Failure(DomainErrors.AnUnexpectedError(nameof(DeleteUserCommand)));
        }

        var result = await this.userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            return Result.Failure(DomainErrors.AnUnexpectedError(nameof(DeleteUserCommand)));
        }

        return Result.Success();
    }
}