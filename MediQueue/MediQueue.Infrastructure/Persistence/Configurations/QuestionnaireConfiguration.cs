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

            builder.Property(x => x.Gender)
                .HasConversion<string>();

            builder.Property(x => x.Balance)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Account)
                .WithOne(a => a.Questionnaire)
                .HasForeignKey<Questionnaire>(x => x.AccountId);
        }
    }
}
