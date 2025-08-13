using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Configurations;
using TravelBooking.Core.Models;

namespace TravelBooking.Repository.Data.Config
{
    public class TourTicketConfig : BaseEntityConfiguration<TourTicket>
    {
        public override void Configure(EntityTypeBuilder<TourTicket> builder)
        {
            base.Configure(builder);

            builder.Property(tt => tt.Type)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(tt => tt.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(tt => tt.AvailableQuantity)
                   .IsRequired();

            builder.Property(tt => tt.IsActive)
                   .IsRequired();

            builder.HasOne(tt => tt.Tour)
                   .WithMany(t => t.TourTickets)
                   .HasForeignKey(tt => tt.TourId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
