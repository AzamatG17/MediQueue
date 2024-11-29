using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class DoctorCabinetConfiguration : IEntityTypeConfiguration<DoctorCabinet>
    {
        public void Configure(EntityTypeBuilder<DoctorCabinet> builder)
        {
            builder.ToTable(nameof(DoctorCabinet));
            builder.HasKey(d => d.Id);

            builder.Property(d => d.IsActive)
                .HasDefaultValue(true);

            builder.Property(d => d.RoomNumber)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(d => d.Account)
                .WithOne(a => a.DoctorCabinet)
                .HasForeignKey<DoctorCabinet>(d => d.AccountId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(d => d.DoctorCabinetLekarstvos)
                .WithOne(dc => dc.DoctorCabinet)
                .HasForeignKey(dc => dc.DoctorCabinetId);
        }
    }
}
