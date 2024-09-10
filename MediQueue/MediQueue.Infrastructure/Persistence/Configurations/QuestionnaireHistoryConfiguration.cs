using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    public class QuestionnaireHistoryConfiguration : IEntityTypeConfiguration<QuestionnaireHistory>
    {
        public void Configure(EntityTypeBuilder<QuestionnaireHistory> builder)
        {
            builder.ToTable(nameof(QuestionnaireHistory));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.HistoryDiscription).IsRequired();

            builder.HasOne(qh => qh.Questionnaire)
                  .WithMany(q => q.QuestionnaireHistories)
                  .HasForeignKey(qh => qh.QuestionnaireId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(qh => qh.Account)
                  .WithMany(a => a.QuestionnaireHistories)
                  .HasForeignKey(qh => qh.AccountId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(qh => qh.Services)
                  .WithMany(s => s.QuestionnaireHistories)
                  .UsingEntity(j => j.ToTable("QuestionnaireHistoryService"));
        }
    }
}
