﻿using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable(nameof(Service));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Category)
                .WithMany(c => c.Services)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(s => s.ServiceUsages)
                .WithOne(qh => qh.Service)
                .HasForeignKey(qh => qh.ServiceId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(s => s.Accounts)
                .WithMany(c => c.Services);
        }
    }
}
