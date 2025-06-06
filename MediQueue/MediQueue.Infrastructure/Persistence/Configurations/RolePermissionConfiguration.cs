﻿using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable(nameof(RolePermission));
            builder.HasKey(rp => rp.Id);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(rp => rp.ControllerId)
                .IsRequired();

            builder.Property(rp => rp.Permissions)
                        .HasConversion(
                        v => JsonConvert.SerializeObject(v),  // Convert List<int> to JSON
                        v => JsonConvert.DeserializeObject<List<int>>(v)  // Convert JSON back to List<int>
                    );

            builder.HasOne(rp => rp.Account)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
