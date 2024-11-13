using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable(nameof(Category));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(c => c.CategoryName)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasMany(c => c.Groups)
                .WithMany(g => g.Categories);

            builder.HasMany(c => c.Services)
                .WithOne(gc => gc.Category)
                .HasForeignKey(gc => gc.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
