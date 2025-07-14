using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Configurations
{
    public class HotelCompanyConfig : BaseEntityConfiguration<HotelCompany>
    {
        public override void Configure(EntityTypeBuilder<HotelCompany> builder)
        {
            base.Configure(builder);

            builder.Property(h => h.Name).IsRequired().HasMaxLength(100);
            builder.Property(h => h.Description).HasMaxLength(1000);
            builder.Property(h => h.Location).HasMaxLength(100); 
        }
    }
}