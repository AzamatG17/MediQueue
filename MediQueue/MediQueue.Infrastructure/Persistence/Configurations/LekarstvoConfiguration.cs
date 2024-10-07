using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class LekarstvoConfiguration : IEntityTypeConfiguration<Lekarstvo>
    {
        public void Configure(EntityTypeBuilder<Lekarstvo> builder)
        {
            builder.ToTable(nameof(Lekarstvo));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

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

            builder.Property(x => x.MeasurementUnit)
                .HasConversion<string>();

            builder.HasOne(x => x.CategoryLekarstvo)
                .WithMany(l => l.Lekarstvos)
                .HasForeignKey(x => x.CategoryLekarstvoId);

            builder.HasOne(x => x.Sclad)
                .WithMany(l => l.Lekarstvos)
                .HasForeignKey(x => x.ScladId);

            builder.Navigation(x => x.CategoryLekarstvo)
                .AutoInclude();

            builder.Navigation(x => x.Sclad)
                .AutoInclude();
        }
    }
}
