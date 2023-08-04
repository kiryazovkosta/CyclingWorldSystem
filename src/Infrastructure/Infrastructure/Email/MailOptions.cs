// ------------------------------------------------------------------------------------------------
//  <copyright file="MailOptions.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Infrastructure.Email;

public class MailOptions
{
    public string Mail { get; init; } = null!;
    public string DisplayName { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string Host { get; init; } = null!;
    public int Port { get; init; }
}