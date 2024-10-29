using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class ScladConfiguration : IEntityTypeConfiguration<Sclad>
    {
        public void Configure(EntityTypeBuilder<Sclad> builder)
        {
            builder.ToTable(nameof(Sclad));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.Name)
                .IsRequired();

            builder.HasOne(x => x.Branch)
                .WithMany(b => b.Sclads)
                .HasForeignKey(x => x.Branchid);

            builder.HasMany(x => x.ScladLekarstvos)
                .WithOne(l => l.Sclad)
                .HasForeignKey(l => l.ScladId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Navigation(x => x.Branch)
                .AutoInclude();
        }   
    }
}
