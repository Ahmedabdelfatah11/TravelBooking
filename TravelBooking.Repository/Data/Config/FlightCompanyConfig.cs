using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Configurations
{
    public class FlightCompanyConfig : BaseEntityConfiguration<FlightCompany>
    {
        public override void Configure(EntityTypeBuilder<FlightCompany> builder)
        {
            base.Configure(builder);

            builder.Property(fc => fc.Name).IsRequired().HasMaxLength(100);
            builder.Property(fc => fc.Description).HasMaxLength(1000);
            builder.Property(fc => fc.ImageUrl).HasMaxLength(500);
            builder.Property(fc => fc.Location).HasMaxLength(100);

            builder.HasOne(h => h.Admin)
         .WithOne(u => u.FlightCompany)
             .HasForeignKey<FlightCompany>(h => h.AdminId)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}