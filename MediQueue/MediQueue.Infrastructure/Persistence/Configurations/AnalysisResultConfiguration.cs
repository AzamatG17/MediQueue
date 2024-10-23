using MediQueue.Domain.Entities;
using MediQueue.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    public class AnalysisResultConfiguration : IEntityTypeConfiguration<AnalysisResult>
    {
        public void Configure(EntityTypeBuilder<AnalysisResult> builder)
        {
            builder.ToTable(nameof(AnalysisResult));
            builder.HasKey(x => x.Id);

            builder.Property(ar => ar.MeasuredValue)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ar => ar.Unit)
                .IsRequired();

            builder.Property(ar => ar.PhotoBase64);

            builder.Property(ar => ar.Status)
                .IsRequired()
                .HasDefaultValue(TestStatus.Pending);

            builder.Property(ar => ar.ResultDate)
                .IsRequired(false);

            builder.HasOne(ar => ar.ServiceUsage)
                .WithMany()
                .HasForeignKey(ar => ar.ServiceUsageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ar => ar.Account)
                .WithMany()
                .HasForeignKey(ar => ar.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ar => ar.QuestionnaireHistory)
                .WithMany(qh => qh.AnalysisResults)
                .HasForeignKey(ar => ar.QuestionnaireHistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
