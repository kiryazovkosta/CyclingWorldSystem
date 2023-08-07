namespace Application.Identity.Users.Commands.UpdateUserDetails;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Identity;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;

public class UpdateUserDetailsCommandHandler
    : ICommandHandler<UpdateUserDetailsCommand, bool>
{
    private readonly UserManager<User> userManager;

    public UpdateUserDetailsCommandHandler(UserManager<User> userManager)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<Result<bool>> Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userManager.FindByNameAsync(request.UserName);
        if (user is null)
        {
            return Result.Failure<bool>(DomainErrors.User.NonExistsUser);           
        }

        user.Update(request.UserName, request.Email, user.EmailConfirmed, request.FirstName, request.LastName);
        var result = await this.userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return Result.Failure<bool>(DomainErrors.User.FailedToUpdateUser);
        }

        return Result.Success<bool>(true);
    }
}