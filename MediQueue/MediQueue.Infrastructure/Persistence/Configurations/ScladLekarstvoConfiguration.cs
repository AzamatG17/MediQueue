using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class ScladLekarstvoConfiguration : IEntityTypeConfiguration<ScladLekarstvo>
    {
        public void Configure(EntityTypeBuilder<ScladLekarstvo> builder)
        {
            builder.ToTable(nameof(ScladLekarstvo));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.Quantity)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(sl => sl.Sclad)
               .WithMany(s => s.ScladLekarstvos)
               .HasForeignKey(sl => sl.ScladId);

            builder.HasOne(sl => sl.Partiya)
                   .WithMany()
                   .HasForeignKey(sl => sl.PartiyaId);
        }
    }
}
