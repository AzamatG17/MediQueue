using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class WardConfiguration : IEntityTypeConfiguration<Ward>
    {
        public void Configure(EntityTypeBuilder<Ward> builder)
        {
            builder.ToTable(nameof(Ward));
            builder.HasKey(x => x.Id);

            builder.Property(e => e.WardName)
                   .HasMaxLength(255);

            builder.HasMany(e => e.WardPlaces)
                   .WithOne(e => e.Ward)
                   .HasForeignKey(e => e.WardId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(w => w.Tariffs)
                .WithMany(e => e.Wards);
        }
    }
}
