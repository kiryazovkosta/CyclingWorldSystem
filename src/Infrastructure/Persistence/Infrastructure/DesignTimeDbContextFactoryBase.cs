// ------------------------------------------------------------------------------------------------
//  <copyright file="DesignTimeDbContextFactoryBase.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Persistence.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public abstract class DesignTimeDbContextFactoryBase<TContext>
    : IDesignTimeDbContextFactory<TContext>
    where TContext : DbContext
{
private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

private const string ConnectionStringName = "DatabaseConnection";

public TContext CreateDbContext(string[] args)
{
    var basePath = Directory.GetCurrentDirectory() + string.Format(
        "{0}..{0}..{0}Presentation{0}WebApi",
        Path.DirectorySeparatorChar);
    string? environmentName = Environment.GetEnvironmentVariable(AspNetCoreEnvironment);
    return Create(basePath, environmentName);
}

protected abstract TContext CreateNewInstance(
    DbContextOptions<TContext> options);

private TContext Create(string basePath, string? environmentName)
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json")
        .AddJsonFile("appsettings.Local.json", true)
        .AddJsonFile($"appsettings.{environmentName}.json", true)
        .AddEnvironmentVariables()
        .Build();

    var connectionString = configuration.GetConnectionString(ConnectionStringName);

    return Create(connectionString);
}

private TContext Create(string? connectionString)
{
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new ArgumentException(
            $"Connection string '{ConnectionStringName}' is null or empty.",
            nameof(connectionString));
    }

    Console.WriteLine(
        $"DesignTimeDbContextFactoryBase.Create(string): Connection string: '{connectionString}'.");

    var optionsBuilder = new DbContextOptionsBuilder<TContext>();

    optionsBuilder.UseSqlServer(connectionString);

    var principalProvider = new DesignTimePrincipalProvider();

    return CreateNewInstance(optionsBuilder.Options);
}


}