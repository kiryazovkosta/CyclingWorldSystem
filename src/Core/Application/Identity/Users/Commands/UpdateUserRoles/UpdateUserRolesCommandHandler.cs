namespace Application.Identity.Users.Commands.UpdateUserRoles;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Identity;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;

public class UpdateUserRolesCommandHandler
    : ICommandHandler<UpdateUserRolesCommand, Guid>
{
    private readonly UserManager<User> userManager;

    public UpdateUserRolesCommandHandler(UserManager<User> userManager)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<Result<Guid>> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Result.Failure<Guid>(DomainErrors.User.NonExistsUser);            
        }
        
        var roles = await this.userManager.GetRolesAsync(user);
        var result = await this.userManager.RemoveFromRolesAsync(user, roles.ToArray());
        if (!result.Succeeded)
        {
            return Result.Failure<Guid>(DomainErrors.User.FailedToRemoveUserRoles);
        }

        if (request.Roles is not null 
            && request.Roles.Any())
        {
            result = await this.userManager.AddToRolesAsync(user, request.Roles);    
        }

        return !result.Succeeded ? 
            Result.Failure<Guid>(DomainErrors.User.FailedToAssignUserRoles) 
            : Result.Success<Guid>(request.UserId);
    }
}