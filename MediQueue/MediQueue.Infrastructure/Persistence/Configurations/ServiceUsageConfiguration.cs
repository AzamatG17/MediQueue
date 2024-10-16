using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    public class ServiceUsageConfiguration : IEntityTypeConfiguration<ServiceUsage>
    {
        public void Configure(EntityTypeBuilder<ServiceUsage> builder)
        {
            builder.ToTable(nameof(ServiceUsage));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.QuantityUsed)
                 .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(lu => lu.Service)
                .WithMany(l => l.ServiceUsages)
                .HasForeignKey(lu => lu.ServiceId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
