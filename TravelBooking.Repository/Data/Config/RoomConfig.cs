using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Configurations
{
    public class RoomConfiguration : BaseEntityConfiguration<Room>
    {
        public override void Configure(EntityTypeBuilder<Room> builder)
        {
            base.Configure(builder);

            builder.Property(r => r.Price).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(r => r.IsAvailable).HasDefaultValue(true);
            builder.Property(r => r.RoomType).IsRequired();

            // one-to-many with HotelCompany
            builder.HasOne(r => r.Hotel)
                   .WithMany(h => h.Rooms)
                   .HasForeignKey(r => r.HotelId)
                   .OnDelete(DeleteBehavior.Cascade); // Cascade delete if HotelCompany is deleted

        }
    }
}