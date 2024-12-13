using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable(nameof(AuditLog));
            builder.HasKey(x => x.Id);

            builder.Property(a => a.Timestamp)
                .IsRequired();

            builder.Property(a => a.TableName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.Action)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(a => a.RecordId)
                .HasMaxLength(4000);

            builder.Property(a => a.Changes)
                .HasMaxLength(4000);

            builder.Property(a => a.AccountId)
                .IsRequired(false);

            builder.HasOne(a => a.Account)
                .WithMany()
                .HasForeignKey(a => a.AccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}
