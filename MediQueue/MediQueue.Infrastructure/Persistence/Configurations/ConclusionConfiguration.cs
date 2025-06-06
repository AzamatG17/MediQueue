﻿using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    public class ConclusionConfiguration : IEntityTypeConfiguration<Conclusion>
    {
        public void Configure(EntityTypeBuilder<Conclusion> builder)
        {
            builder.ToTable(nameof(Conclusion));
            builder.HasKey(c => c.Id);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(c => c.Discription);

            builder.HasOne(c => c.ServiceUsage)
               .WithMany()
               .HasForeignKey(c => c.ServiceUsageId);

            builder.HasOne(c => c.Account)
               .WithMany()
               .HasForeignKey(c => c.AccountId)
               .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(c => c.QuestionnaireHistory)
               .WithMany(qh => qh.Conclusions)
               .HasForeignKey(c => c.QuestionnaireHistoryId)
               .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
