namespace Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

//public abstract class ApplicationDbContextFactory
//	: IDesignTimeDbContextFactory<ApplicationDbContext>
//{
//	public ApplicationDbContext CreateDbContext(string[] args)
//	{
//		var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
//		optionsBuilder.UseSqlServer("Server=.;Database=CyclingWorldSystemDb;Integrated Security=true;MultipleActiveResultSets=true;Encrypt=False;TrustServerCertificate=True");

//		return new ApplicationDbContext(optionsBuilder.Options);
//	}
//}