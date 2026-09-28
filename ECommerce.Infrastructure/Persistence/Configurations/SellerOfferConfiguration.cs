using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class SellerOfferConfiguration : IEntityTypeConfiguration<SellerOffer>
{
    public void Configure(EntityTypeBuilder<SellerOffer> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Price).HasPrecision(18, 2).IsRequired();
        builder.Property(x => x.Stock).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.RowVersion).IsRowVersion();

        builder.HasOne(x => x.ProductVariant)
            .WithMany(x => x.SellerOffers)
            .HasForeignKey(x => x.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.SellerId, x.ProductVariantId }).IsUnique();
    }
}