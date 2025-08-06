using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Configurations
{
    public class TourCompanyConfig : BaseEntityConfiguration<TourCompany>
    {
        public override void Configure(EntityTypeBuilder<TourCompany> builder)
        {
            base.Configure(builder);

            builder.Property(tc => tc.Name).IsRequired().HasMaxLength(100);
            builder.Property(tc => tc.Description).HasMaxLength(1000);
            builder.Property(tc => tc.ImageUrl).HasMaxLength(500);
            builder.Property(tc => tc.Rating).IsRequired(false);
            builder.Property(tc => tc.Location).HasMaxLength(100);
            builder.HasOne(h => h.Admin)
            .WithOne(u => u.TourCompany)
         .HasForeignKey<TourCompany>(h => h.AdminId)
          .OnDelete(DeleteBehavior.Restrict);
        }
    }
}