using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class SampleConfiguration : IEntityTypeConfiguration<Sample>
    {
        public void Configure(EntityTypeBuilder<Sample> builder)
        {
            builder.ToTable(nameof(Sample));
            builder.HasKey(x => x.Id);

            builder.Property(a => a.IsActive)
                .HasDefaultValue(true);

            builder.Property(a => a.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.Description)
                .IsRequired();
        }
    }
}
