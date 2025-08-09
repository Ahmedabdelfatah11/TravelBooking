using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Configurations
{
    public class TourConfig: BaseEntityConfiguration<Tour>
    {
        public override void Configure(EntityTypeBuilder<Tour> builder)
        {
            base.Configure(builder);

            builder.Property(t => t.StartDate).IsRequired();
            builder.Property(t => t.EndDate).IsRequired();
            builder.Property(t => t.Description).HasMaxLength(1000);
            builder.Property(t => t.Destination).HasMaxLength(100);
            builder.Property(t => t.Price).HasColumnType("decimal(18,2)");
            builder.Property(t => t.MaxGuests).IsRequired();
            builder.Property(t => t.Category).HasConversion<string>();

            // one-to-many with TourCompany
            builder.HasOne(t => t.TourCompany)
                   .WithMany(tc => tc.Tours)
                   .HasForeignKey(t => t.TourCompanyId)
                   .OnDelete(DeleteBehavior.Restrict);


        }
    }
}