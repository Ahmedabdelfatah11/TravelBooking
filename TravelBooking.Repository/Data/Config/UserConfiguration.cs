using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Models;

namespace TravelBooking.Core.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            // One-to-many with bookings
            builder.HasMany(u => u.bookings)
                   .WithOne(b => b.User)
                   .HasForeignKey(b => b.UserId)
                   .IsRequired(false); 
        }
    }
}
