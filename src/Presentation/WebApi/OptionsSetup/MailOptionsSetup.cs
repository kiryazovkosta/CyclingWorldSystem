// ------------------------------------------------------------------------------------------------
//  <copyright file="MailOptionsSetup.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace WebApi.OptionsSetup;

using Infrastructure.Email;
using Microsoft.Extensions.Options;

public class MailOptionsSetup : IConfigureOptions<MailOptions>
{
    private const string SectionName = "MailSettings";
    private readonly IConfiguration _configuration;

    public MailOptionsSetup(IConfiguration configuration)
    {
        this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public void Configure(MailOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}