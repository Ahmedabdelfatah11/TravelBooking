using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Repository.Data.Config
{
    public class TourBookingTicketConfig : IEntityTypeConfiguration<TourBookingTicket>
    {
        public void Configure(EntityTypeBuilder<TourBookingTicket> builder)
        {
            builder.HasKey(tbt => tbt.Id);

            builder.Property(tbt => tbt.Quantity)
                   .IsRequired();

            builder.Property(tbt => tbt.IsIssued)
                   .IsRequired();

            builder.HasOne(tbt => tbt.Booking)
                   .WithMany(b => b.BookingTickets)
                   .HasForeignKey(tbt => tbt.BookingId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tbt => tbt.Ticket)
                   .WithMany()
                   .HasForeignKey(tbt => tbt.TicketId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
