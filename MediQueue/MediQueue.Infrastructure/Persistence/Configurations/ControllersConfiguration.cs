using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    public class ControllersConfiguration : IEntityTypeConfiguration<Controllers>
    {
        public void Configure(EntityTypeBuilder<Controllers> builder)
        {
            builder.ToTable(nameof(Controllers));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ControllerName)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
