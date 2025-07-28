using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Configurations
{
    public class BookingConfig : BaseEntityConfiguration<Booking>
    {
        public override void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Property(b => b.StartDate).IsRequired();
            builder.Property(b => b.EndDate).IsRequired();
            builder.Property(b => b.Status).IsRequired();
            builder.Property(b => b.BookingType).IsRequired();

            builder.HasOne(b => b.Room)
                   .WithMany()
                   .HasForeignKey(b => b.RoomId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Car)
                   .WithMany()
                   .HasForeignKey(b => b.CarId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Flight)
                   .WithMany()
                   .HasForeignKey(b => b.FlightId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Tour)
                   .WithMany()
                   .HasForeignKey(b => b.TourId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.User)
                   .WithMany()
                   .HasForeignKey(b => b.UserId)
                   .IsRequired(false);
        }
    }
}
