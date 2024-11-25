using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    public class LekarstvoConfiguration : IEntityTypeConfiguration<Lekarstvo>
    {
        public void Configure(EntityTypeBuilder<Lekarstvo> builder)
        {
            builder.ToTable(nameof(Lekarstvo));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.PhotoBase64)
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.MeasurementUnit)
                .HasConversion<string>();

            builder.HasOne(x => x.CategoryLekarstvo)
                .WithMany(l => l.Lekarstvos)
                .HasForeignKey(x => x.CategoryLekarstvoId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Navigation(x => x.CategoryLekarstvo)
                .AutoInclude();
        }
    }
}
