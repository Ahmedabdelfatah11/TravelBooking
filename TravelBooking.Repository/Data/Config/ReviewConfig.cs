using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Configurations;
using TravelBooking.Core.Models;

namespace TravelBooking.Repository.Data.Config
{
    public class ReviewConfig : BaseEntityConfiguration<Review>
    {
        public override void Configure(EntityTypeBuilder<Review> builder)
        {
            base.Configure(builder);

            builder.HasOne(f => f.User)
                            .WithMany()
                            .HasForeignKey(f => f.UserId)
                            .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(f => f.HotelCompany)
                .WithMany()
                .HasForeignKey(f => f.HotelCompanyId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(f => f.FlightCompany)
                  .WithMany()
                  .HasForeignKey(f => f.FlightCompanyId)
                  .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(f => f.CarRentalCompany)
                  .WithMany()
                  .HasForeignKey(f => f.CarRentalCompanyId)
                  .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(f => f.TourCompany)
                  .WithMany()
                  .HasForeignKey(f => f.TourCompanyId)
                  .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(f => new { f.UserId, f.HotelCompanyId, f.FlightCompanyId, f.CarRentalCompanyId, f.TourCompanyId }).IsUnique();
        }

    }
}
