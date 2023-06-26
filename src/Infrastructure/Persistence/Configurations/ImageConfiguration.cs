namespace Persistence.Configurations
{
	using Common.Constants;
	using Domain.Entities;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	public class ImageConfiguration : IEntityTypeConfiguration<Image>
	{
		public void Configure(EntityTypeBuilder<Image> builder)
		{
			builder
				.ToTable(GlobalConstants.Image.TableName);

			builder
				.HasKey(i => i.Id);

			builder
				.Property(i => i.Url)
				.HasMaxLength(GlobalConstants.Image.UrlMaxLength)
				.IsRequired();

			builder
				.HasOne(i => i.Activity)
				.WithMany(a => a.Images)
				.HasForeignKey(i => i.ActivityId);
		}
	}
}