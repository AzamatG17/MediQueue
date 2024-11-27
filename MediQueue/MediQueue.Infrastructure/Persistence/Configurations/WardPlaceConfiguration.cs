using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class WardPlaceConfiguration : IEntityTypeConfiguration<WardPlace>
    {
        public void Configure(EntityTypeBuilder<WardPlace> builder)
        {
            builder.ToTable(nameof(WardPlace));
            builder.HasKey(x => x.Id);

            builder.Property(e => e.IsOccupied)
                   .HasDefaultValue(false);

            builder.HasOne(e => e.Ward)
                   .WithMany(w => w.WardPlaces)
                   .HasForeignKey(e => e.WardId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.StationaryStay)
                   .WithOne(s => s.WardPlace)
                   .HasForeignKey<WardPlace>(e => e.StationaryStayId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
