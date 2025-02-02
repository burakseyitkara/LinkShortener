using LinkShortener.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkShortener.Infrastructure.Data.Configurations;

/// <summary>
/// Link entity konfigürasyonu
/// </summary>
public class LinkConfiguration : IEntityTypeConfiguration<Link>
{
    public void Configure(EntityTypeBuilder<Link> builder)
    {
        builder.ToTable("Links");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OriginalUrl)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(x => x.ShortCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(x => x.ShortCode)
            .IsUnique();

        builder.Property(x => x.Title)
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.ClickCount)
            .HasDefaultValue(0);

        // Soft delete filtresi
        builder.HasQueryFilter(x => !x.DeletedAt.HasValue);

        // İlişkiler
        builder.HasOne(x => x.User)
            .WithMany(x => x.Links)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.Clicks)
            .WithOne(x => x.Link)
            .HasForeignKey(x => x.LinkId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 