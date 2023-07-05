namespace Persistence.Configurations;

using Common.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BikeTypeConfiguration : IEntityTypeConfiguration<BikeType>
{
	public void Configure(EntityTypeBuilder<BikeType> builder)
	{
		builder.ToTable(GlobalConstants.BikeType.TableName);

		builder.HasQueryFilter(bikeType => !bikeType.IsDeleted);

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.HasMaxLength(GlobalConstants.BikeType.NameMaxLength)
			.IsRequired();

		builder.HasMany(bikeType => bikeType.Bikes)
			.WithOne(bike => bike.BikeType)
			.HasForeignKey(bike => bike.BikeTypeId)
			.IsRequired();
	}
}