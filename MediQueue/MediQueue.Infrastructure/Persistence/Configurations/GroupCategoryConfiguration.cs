using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class GroupCategoryConfiguration : IEntityTypeConfiguration<GroupCategory>
    {
        public void Configure(EntityTypeBuilder<GroupCategory> builder)
        {
            builder.HasKey(e => new { e.GroupId, e.CategoryId });

            builder.HasOne(e => e.Group)
                .WithMany(g => g.GroupCategories)
                .HasForeignKey(e => e.GroupId);

            builder.HasOne(e => e.Category)
                .WithMany(c => c.GroupCategories)
                .HasForeignKey(e => e.CategoryId);
        }
    }
}
