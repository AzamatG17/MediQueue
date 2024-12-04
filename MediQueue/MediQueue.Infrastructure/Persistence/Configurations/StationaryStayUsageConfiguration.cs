using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class StationaryStayUsageConfiguration : IEntityTypeConfiguration<StationaryStayUsage>
    {
        public void Configure(EntityTypeBuilder<StationaryStayUsage> builder)
        {
            builder.ToTable(nameof(StationaryStayUsage));
            builder.HasKey(x => x.Id);

            builder.Property(e => e.StartTime)
                   .HasColumnType("datetime");

            builder.Property(e => e.NumberOfDays)
                   .HasDefaultValue(null);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.QuantityUsed)
                 .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(e => e.Tariff)
                   .WithMany()
                   .HasForeignKey(e => e.TariffId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.WardPlace)
                   .WithOne(w => w.StationaryStay)
                   .HasForeignKey<StationaryStayUsage>(e => e.WardPlaceId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.Nutrition)
                   .WithMany()
                   .HasForeignKey(e => e.NutritionId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.QuestionnaireHistory)
                   .WithMany(qh => qh.StationaryStays)
                   .HasForeignKey(e => e.QuestionnaireHistoryId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
