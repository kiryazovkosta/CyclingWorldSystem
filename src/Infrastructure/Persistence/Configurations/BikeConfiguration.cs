namespace Persistence.Configurations;

using Common.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class BikeConfiguration : IEntityTypeConfiguration<Bike>
{
	public void Configure(EntityTypeBuilder<Bike> builder)
	{
		builder.ToTable("Bikes");

		builder.HasKey(x => x.Id);

		builder.HasQueryFilter(bike => !bike.IsDeleted);

		builder
			.Property(bike => bike.Brand)
			.HasMaxLength(GlobalConstants.Bike.BrandMaxLength)
			.IsRequired();
		builder.Property(bike => bike.Model)
			.HasMaxLength(GlobalConstants.Bike.ModelMaxLength)
			.IsRequired();
	}
}