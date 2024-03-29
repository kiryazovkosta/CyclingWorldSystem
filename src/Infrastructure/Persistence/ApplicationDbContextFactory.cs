﻿namespace Persistence;

using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class ApplicationDbContextFactory : DesignTimeDbContextFactoryBase<ApplicationDbContext>
{
	protected override ApplicationDbContext CreateNewInstance(DbContextOptions<ApplicationDbContext> options)
	{
		return new ApplicationDbContext(options);
	}
}