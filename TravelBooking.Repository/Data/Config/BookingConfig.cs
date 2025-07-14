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
            builder.Property(b => b.status).IsRequired();
            builder.Property(b => b.BookingType).IsRequired();
            builder.Property(b => b.RefId).IsRequired(false);

            // One-to-many relationship with user
            builder.HasOne(b => b.User)
                   .WithMany()
                   .HasForeignKey(b => b.UserId);

            // one to many relationship with payment
            builder.HasOne(b => b.Payment)
                   .WithMany()
                   .HasForeignKey(b => b.PaymentId);
        }
    }
}