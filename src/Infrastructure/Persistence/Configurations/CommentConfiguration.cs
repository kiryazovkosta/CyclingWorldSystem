namespace Persistence.Configurations;

using Common.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
	public void Configure(EntityTypeBuilder<Comment> builder)
	{
		builder
			.ToTable(GlobalConstants.Comment.TableName);

		builder
			.HasKey(x => x.Id);

		builder.Property(c => c.Content)
			.HasMaxLength(GlobalConstants.Comment.ContentMaxLength)
			.IsRequired();

		builder.HasOne(c => c.User)
			.WithMany(u => u.Comments)
			.IsRequired()
			.HasForeignKey(c => c.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(c => c.Activity)
			.WithMany(a => a.Comments)
			.HasForeignKey(c => c.ActivityId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}