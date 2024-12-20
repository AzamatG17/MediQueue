using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class ProcedureConfiguration : IEntityTypeConfiguration<Procedure>
    {
        public void Configure(EntityTypeBuilder<Procedure> builder)
        {
            builder.ToTable(nameof(Procedure));
            builder.HasKey(x => x.Id);

            builder.Property(a => a.Name)
                .HasMaxLength(255);

            builder.Property(a => a.Description);

            builder.Property(e => e.StartTime)
                .HasColumnType("TIME");

            builder.Property(e => e.EndTime)
                .HasColumnType("TIME");

            builder.Property(e => e.IntervalDuration);

            builder.Property(e => e.BreakDuration);

            builder.Property(e => e.MaxPatients);

            builder.HasOne(e => e.ProcedureCategory)
                .WithMany(c => c.Procedures)
                .HasForeignKey(e => e.ProcedureCategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.ProcedureBookings)
                .WithOne(b => b.Procedure)
                .HasForeignKey(b => b.ProcedureId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
