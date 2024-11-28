using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class TariffConfiguration : IEntityTypeConfiguration<Tariff>
    {
        public void Configure(EntityTypeBuilder<Tariff> builder)
        {
            builder.ToTable(nameof(Tariff));
            builder.HasKey(x => x.Id);

            builder.Property(e => e.Name)
                   .HasMaxLength(255);

            builder.Property(e => e.PricePerDay)
                   .HasColumnType("decimal(18,2)");

            builder.HasMany(t => t.Wards)
                .WithMany(w => w.Tariffs);
        }
    }
}
