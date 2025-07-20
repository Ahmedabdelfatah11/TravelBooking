using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Configurations
{
    public class CarConfig : BaseEntityConfiguration<CarDTO>
    {
        public override void Configure(EntityTypeBuilder<CarDTO> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.Model).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Price).HasColumnType("decimal(18,2)");
            builder.Property(c => c.Description).HasMaxLength(1000);
            builder.Property(c => c.IsAvailable).HasDefaultValue(true);
            builder.Property(c => c.Location).HasMaxLength(100);
            builder.Property(c => c.ImageUrl).HasMaxLength(500);
            builder.Property(c => c.Capacity).HasDefaultValue(5);

            // one-to-many with CarRentalCompany
            builder.HasOne(c => c.RentalCompany)
                   .WithMany(crc => crc.Cars)
                   .HasForeignKey(c => c.RentalCompanyId)
                   .OnDelete(DeleteBehavior.Cascade); // Cascade delete if CarRentalCompany is deleted

        }
    }
}