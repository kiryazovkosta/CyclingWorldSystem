// ------------------------------------------------------------------------------------------------
//  <copyright file="UserDto.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Domain.Identity.Dtos;

public sealed class UserDto
{
    public Guid Id { get; init; }
    public string UserName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public bool EmailConfirmed { get; init; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string ImageUrl { get; init; } = null!;
    public List<string> Roles { get; init; } = null!;
}
