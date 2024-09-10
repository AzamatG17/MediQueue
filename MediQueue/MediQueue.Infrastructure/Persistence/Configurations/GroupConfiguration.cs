using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Group = MediQueue.Domain.Entities.Group;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class GroupConfiguration : IEntityTypeConfiguration<Domain.Entities.Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable(nameof(Domain.Entities.Group));
            builder.HasKey(x => x.Id);

            builder.Property(g => g.GroupName)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasMany(g => g.Categories)
                .WithMany(gc => gc.Groups);
        }
    }
}
