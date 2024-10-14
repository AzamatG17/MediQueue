using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    public class PaymentLekarstvoConfiguration : IEntityTypeConfiguration<PaymentLekarstvo>
    {
        public void Configure(EntityTypeBuilder<PaymentLekarstvo> builder)
        {
            builder.ToTable(nameof(PaymentLekarstvo));
            builder.HasKey(pl => pl.Id);

            builder.Property(pl => pl.TotalAmount)
               .HasColumnType("decimal(18,2)");

            builder.Property(pl => pl.PaidAmount)
                   .HasColumnType("decimal(18,2)");

            builder.Property(pl => pl.OutstandingAmount)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(pl => pl.Account)
                   .WithMany()
                   .HasForeignKey(pl => pl.AccountId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(pl => pl.Lekarstvo)
                   .WithMany()
                   .HasForeignKey(pl => pl.LekarstvoId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
