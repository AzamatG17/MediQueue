using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class StationaryStayConfiguration : IEntityTypeConfiguration<StationaryStay>
    {
        public void Configure(EntityTypeBuilder<StationaryStay> builder)
        {
            builder.ToTable(nameof(StationaryStay));
            builder.HasKey(x => x.Id);

            builder.Property(e => e.StartTime)
                   .HasColumnType("datetime");

            builder.Property(e => e.NumberOfDays)
                   .HasDefaultValue(null);

            builder.Property(e => e.TotalCost)
                .HasColumnType("decimal(18, 2)");

            builder.HasOne(e => e.Tariff)
                   .WithMany()
                   .HasForeignKey(e => e.TariffId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.WardPlace)
                   .WithOne(w => w.StationaryStay)
                   .HasForeignKey<StationaryStay>(e => e.WardPlaceId)
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
