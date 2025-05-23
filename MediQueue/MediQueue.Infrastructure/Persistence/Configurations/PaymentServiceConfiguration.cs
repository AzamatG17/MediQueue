﻿using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    public class PaymentServiceConfiguration : IEntityTypeConfiguration<PaymentService>
    {
        public void Configure(EntityTypeBuilder<PaymentService> builder)
        {
            builder.ToTable(nameof(PaymentService));
            builder.HasKey(ps => ps.Id);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(ps => ps.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(ps => ps.PaidAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(ps => ps.OutstandingAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(ps => ps.PaymentDate);

            builder.Property(ps => ps.PaymentType)
                .HasConversion<string>();

            builder.Property(ps => ps.PaymentStatus)
                .HasConversion<string>();

            builder.Property(ps => ps.MedicationType)
                .HasConversion<string>();

            builder.HasOne(ps => ps.Service)
                .WithMany()
                .HasForeignKey(ps => ps.ServiceId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(ps => ps.DoctorCabinetLekarstvo)
                .WithMany()
                .HasForeignKey(ps => ps.DoctorCabinetLekarstvoId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(ps => ps.StationaryStayUsage)
                .WithMany(ssu => ssu.PaymentServices)
                .HasForeignKey(ps => ps.StationaryStayUsageId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(ps => ps.QuestionnaireHistory)
                .WithMany(qh => qh.PaymentServices)
                .HasForeignKey(ps => ps.QuestionnaireHistoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(ps => ps.Account)
                .WithMany()
                .HasForeignKey(ps => ps.AccountId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
