using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class AccountEntityConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable(nameof(Account));
            builder.HasKey(x => x.Id);

            builder.Property(a => a.IsActive)
                .HasDefaultValue(true);

            builder.Property(a => a.Login)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.Password)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.Passport)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.FirstName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.LastName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.SurName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.PhotoBase64);

            builder.Property(a => a.PhoneNumber)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasOne(a => a.Role)
                .WithMany(r => r.Accounts)
                .HasForeignKey(a => a.RoleId);

            builder.HasMany(a => a.QuestionnaireHistories)
                .WithOne(h => h.Account)
                .HasForeignKey(h => h.AccountId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(r => r.RolePermissions)
                .WithOne(rp => rp.Account)
                .HasForeignKey(rp => rp.AccountId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.DoctorCabinet)
                .WithOne(d => d.Account)
                .HasForeignKey<Account>(a => a.DoctorCabinetId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
