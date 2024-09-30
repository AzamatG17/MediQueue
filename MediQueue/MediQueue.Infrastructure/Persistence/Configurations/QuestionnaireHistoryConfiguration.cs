using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    public class QuestionnaireHistoryConfiguration : IEntityTypeConfiguration<QuestionnaireHistory>
    {
        public void Configure(EntityTypeBuilder<QuestionnaireHistory> builder)
        {
            builder.ToTable(nameof(QuestionnaireHistory));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.HistoryDiscription).IsRequired();

            builder.Property(qh => qh.Balance)
                .HasColumnType("decimal(18, 2)");

            builder.HasOne(qh => qh.Questionnaire)
                  .WithMany(q => q.QuestionnaireHistories)
                  .HasForeignKey(qh => qh.QuestionnaireId)
                  .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(qh => qh.Account)
                  .WithMany(a => a.QuestionnaireHistories)
                  .HasForeignKey(qh => qh.AccountId)
                  .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(qh => qh.Services)
                  .WithMany(s => s.QuestionnaireHistories)
                  .UsingEntity(j => j.ToTable("QuestionnaireHistoryService"));

            builder.HasMany(qh => qh.PaymentServices)
                .WithOne(ps => ps.QuestionnaireHistory)
                .HasForeignKey(ps => ps.QuestionnaireHistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
