using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Configurations
{
    public class CarRentalConfig : BaseEntityConfiguration<CarRentalCompany>
    {
        public override void Configure(EntityTypeBuilder<CarRentalCompany> builder)
        {
            base.Configure(builder);

            builder.Property(cr => cr.Name).IsRequired().HasMaxLength(100);
            builder.Property(cr => cr.description).HasMaxLength(1000);
            builder.Property(cr => cr.Location).HasMaxLength(100);
            builder.Property(cr => cr.ImageUrl).HasMaxLength(500);

            builder.HasOne(h => h.Admin)
            .WithOne(u => u.CarRentalCompany)
            .HasForeignKey<CarRentalCompany>(h => h.AdminId)
            .IsRequired(false)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}