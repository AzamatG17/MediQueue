using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class PartiyaConfiguration : IEntityTypeConfiguration<Partiya>
    {
        public void Configure(EntityTypeBuilder<Partiya> builder)
        {
            builder.ToTable(nameof(Partiya));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.PurchasePrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.SalePrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TotalQuantity)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.PriceQuantity)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.ExpirationDate)
                .HasColumnType("datetime");

            builder.Property(x => x.BeforeDate)
                .HasColumnType("datetime");

            builder.Property(x => x.PhotoBase64)
                .HasColumnType("nvarchar(max)");

            builder.HasOne(x => x.Lekarstvo)
                .WithMany(l => l.Partiyas)
                .HasForeignKey(x => x.LekarstvoId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Sclad)
                .WithMany(s => s.Partiyas)
                .HasForeignKey(x => x.ScladId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
