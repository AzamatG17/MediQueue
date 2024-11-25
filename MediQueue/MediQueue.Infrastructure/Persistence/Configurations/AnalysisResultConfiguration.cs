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

            builder.Property(ar => ar.IsActive)
                .HasDefaultValue(true);

            builder.Property(ar => ar.MeasuredValue)
                .IsRequired();

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

            builder.HasOne(ar => ar.FirstDoctor)
                .WithMany()
                .HasForeignKey(ar => ar.FirstDoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ar => ar.SecondDoctor)
                .WithMany()
                .HasForeignKey(ar => ar.SecondDoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ar => ar.QuestionnaireHistory)
                .WithMany(qh => qh.AnalysisResults)
                .HasForeignKey(ar => ar.QuestionnaireHistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
