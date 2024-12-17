using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class ProcedureCategoryConfiguration : IEntityTypeConfiguration<ProcedureCategory>
    {
        public void Configure(EntityTypeBuilder<ProcedureCategory> builder)
        {
            builder.ToTable(nameof(ProcedureCategory));
            builder.HasKey(x => x.Id);

            builder.Property(a => a.Name)
                .HasMaxLength(255);

            builder.HasMany(e => e.Procedures)
                .WithOne(p => p.ProcedureCategory)
                .HasForeignKey(p => p.ProcedureCategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
