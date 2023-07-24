// ------------------------------------------------------------------------------------------------
//  <copyright file="RoleDto.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Domain.Identity.Dtos;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; init; } = null!;
    public List<string?> Users { get; init; } = null!;
}