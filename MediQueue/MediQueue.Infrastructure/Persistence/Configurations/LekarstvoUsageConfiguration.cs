using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    public class LekarstvoUsageConfiguration : IEntityTypeConfiguration<LekarstvoUsage>
    {
        public void Configure(EntityTypeBuilder<LekarstvoUsage> builder)
        {
            builder.ToTable(nameof(LekarstvoUsage));
            builder.HasKey(lu => lu.Id);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(lu => lu.QuantityUsed)
                .HasColumnType("decimal(18,2)");

            builder.Property(lu => lu.TotalPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(lu => lu.Amount)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(lu => lu.Conclusion)
                .WithMany(c => c.LekarstvoUsages)
                .HasForeignKey(lu => lu.ConclusionId);

            builder.HasOne(lu => lu.Lekarstvo)
                .WithMany(l => l.LekarstvoUsages)
                .HasForeignKey(lu => lu.LekarstvoId);
        }
    }
}
