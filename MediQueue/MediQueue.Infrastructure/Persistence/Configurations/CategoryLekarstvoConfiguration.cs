using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class CategoryLekarstvoConfiguration : IEntityTypeConfiguration<CategoryLekarstvo>
    {
        public void Configure(EntityTypeBuilder<CategoryLekarstvo> builder)
        {
            builder.ToTable(nameof(CategoryLekarstvo));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.Name)
                .HasMaxLength(256)
                .IsRequired();

            builder.HasMany(x => x.Lekarstvos)
                .WithOne(l => l.CategoryLekarstvo)
                .HasForeignKey(l => l.CategoryLekarstvoId);
        }
    }
}
