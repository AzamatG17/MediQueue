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

            builder.Property(x => x.QuestionnaireId)
                .HasMaxLength(255);

            builder.Property(a => a.PassportSeria)
                .HasMaxLength(255);

            builder.Property(a => a.PassportPinfl)
                .HasMaxLength(255);

            builder.Property(x => x.Gender)
                .HasConversion<string>();

            builder.Property(a => a.FirstName)
                .HasMaxLength(255);

            builder.Property(a => a.LastName)
                .HasMaxLength(255);

            builder.Property(a => a.SurName)
                .HasMaxLength(255);

            builder.Property(a => a.PhoneNumber)
                .HasMaxLength(255);

            builder.Property(a => a.Region)
                .HasMaxLength(255);

            builder.Property(a => a.District)
                .HasMaxLength(255);

            builder.Property(a => a.Posolos)
                .HasMaxLength(255);

            builder.Property(a => a.Address)
                .HasMaxLength(255);

            builder.Property(a => a.DateIssue);

            builder.Property(a => a.DateBefore);

            builder.Property(a => a.Balance)
                .HasColumnType("decimal(18,2)");

            builder.Property(a => a.SocialSattus)
                .HasMaxLength(255);

            builder.Property(a => a.AdvertisingChannel)
                .HasMaxLength(255);

            builder.Property(x => x.PhotoBase64)
                .HasColumnType("nvarchar(max)");

            builder.HasMany(q => q.QuestionnaireHistories)
                .WithOne(h => h.Questionnaire)
                .HasForeignKey(h => h.QuestionnaireId);
        }
    }
}
