using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.ToTable(nameof(Discount));
            builder.HasKey(x => x.Id);

            builder.Property(a => a.IsActive)
                .HasDefaultValue(true);

            builder.Property(a => a.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.Percent)
                .IsRequired();

            builder.HasOne(a => a.QuestionnaireHistory)
                .WithMany(qh => qh.Discounts)
                .HasForeignKey(a => a.QuestionnaireHistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
