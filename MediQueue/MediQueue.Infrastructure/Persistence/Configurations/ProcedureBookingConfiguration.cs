using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class ProcedureBookingConfiguration : IEntityTypeConfiguration<ProcedureBooking>
    {
        public void Configure(EntityTypeBuilder<ProcedureBooking> builder)
        {
            builder.ToTable(nameof(ProcedureBooking));
            builder.HasKey(x => x.Id);

            builder.Property(e => e.BookingDate)
                .HasColumnType("datetime");

            builder.HasOne(e => e.Procedure)
                .WithMany(pb => pb.ProcedureBookings)
                .HasForeignKey(e => e.ProcedureId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.StationaryStayUsage)
                .WithMany(pb => pb.ProcedureBookings)
                .HasForeignKey(e => e.StationaryStayUsageId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
