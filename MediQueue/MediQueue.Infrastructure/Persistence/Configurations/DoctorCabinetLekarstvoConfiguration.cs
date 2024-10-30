using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class DoctorCabinetLekarstvoConfiguration : IEntityTypeConfiguration<DoctorCabinetLekarstvo>
    {
        public void Configure(EntityTypeBuilder<DoctorCabinetLekarstvo> builder)
        {
            builder.ToTable(nameof(DoctorCabinetLekarstvo));
            builder.HasKey(d => d.Id);

            builder.Property(d => d.IsActive)
                .HasDefaultValue(true);

            builder.Property(d => d.Quantity)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(d => d.DoctorCabinet)
                .WithMany(dc => dc.DoctorCabinetLekarstvos)
                .HasForeignKey(d => d.DoctorCabinetId);

            builder.HasOne(d => d.Partiya)
                .WithMany(p => p.DoctorCabinetLekarstvos)
                .HasForeignKey(d => d.PartiyaId);
        }
    }
}
