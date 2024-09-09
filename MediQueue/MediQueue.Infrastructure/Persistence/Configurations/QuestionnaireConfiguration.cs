using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class QuestionnaireConfiguration : IEntityTypeConfiguration<Questionnaire>
    {
        public void Configure(EntityTypeBuilder<Questionnaire> builder)
        {
            builder.ToTable(nameof(Questionnaire));
            builder.HasKey(x => x.Id);

            builder.Property(a => a.Passport)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Gender)
                .HasConversion<string>();

            builder.Property(a => a.FirstName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.LastName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.SurName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.PhoneNumber)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.Region)
                .IsRequired();

            builder.Property(x => x.Balance)
                .HasColumnType("decimal(18,2)");

            builder.HasMany(q => q.QuestionnaireHistories)
                .WithOne(h => h.Questionnaire)
                .HasForeignKey(h => h.QuestionnaireId);
  
        }
    }
}
