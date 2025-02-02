using LinkShortener.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkShortener.Infrastructure.Data.Configurations;

/// <summary>
/// Link tıklanma kaydı entity konfigürasyonu
/// </summary>
public class LinkClickConfiguration : IEntityTypeConfiguration<LinkClick>
{
    public void Configure(EntityTypeBuilder<LinkClick> builder)
    {
        builder.ToTable("LinkClicks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IpAddress)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.UserAgent)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Referer)
            .HasMaxLength(2048);

        builder.Property(x => x.Country)
            .HasMaxLength(100);

        builder.Property(x => x.City)
            .HasMaxLength(100);

        builder.Property(x => x.DeviceType)
            .HasMaxLength(50);

        builder.Property(x => x.OperatingSystem)
            .HasMaxLength(50);

        // Soft delete filtresi
        builder.HasQueryFilter(x => !x.DeletedAt.HasValue);

        // İlişkiler
        builder.HasOne(x => x.Link)
            .WithMany(x => x.Clicks)
            .HasForeignKey(x => x.LinkId)
            .OnDelete(DeleteBehavior.Cascade);

        // İndeksler
        builder.HasIndex(x => x.IpAddress);
        builder.HasIndex(x => x.Country);
        builder.HasIndex(x => x.DeviceType);
        builder.HasIndex(x => x.CreatedAt);
    }
} 