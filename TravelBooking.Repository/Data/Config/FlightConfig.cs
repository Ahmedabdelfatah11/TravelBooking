﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Configurations
{
    public class FlightConfig : BaseEntityConfiguration<Flight>
    {
        public override void Configure(EntityTypeBuilder<Flight> builder)
        {
            base.Configure(builder);

            builder.Property(f => f.DepartureTime).IsRequired();
            builder.Property(f => f.ArrivalTime).IsRequired();
            builder.Property(f => f.Price).HasColumnType("decimal(18,2)");
            builder.Property(f => f.DepartureAirport).HasMaxLength(10);
            builder.Property(f => f.ArrivalAirport).HasMaxLength(10);

            // one-to-many with FlightCompany
            builder.HasOne(f => f.FlightCompany)
                   .WithMany(fc => fc.Flights)
                   .HasForeignKey(f => f.FlightCompanyId);
        }
    }
}