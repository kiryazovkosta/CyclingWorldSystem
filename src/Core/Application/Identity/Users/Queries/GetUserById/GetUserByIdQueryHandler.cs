namespace Application.Identity.Users.Queries.GetUserById;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Identity;
using Domain.Shared;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Models;

public class GetUserByIdQueryHandler
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    private readonly UserManager<User> userManager;

    public GetUserByIdQueryHandler(UserManager<User> userManager)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<Result<UserResponse>> Handle(
        GetUserByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var user = await this.userManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
        {
            return Result.Failure<UserResponse>(DomainErrors.User.NonExistsUser);
        }

        var response = user.Adapt<UserResponse>();
        return response;
    }
}