using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
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

            // ✅ ValueConverter for List<string>
            var stringListConverter = new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>()
            );

            builder.Property(t => t.IncludedItems)
                   .HasConversion(stringListConverter)
                   .HasColumnType("nvarchar(max)");

            builder.Property(t => t.ExcludedItems)
                   .HasConversion(stringListConverter)
                   .HasColumnType("nvarchar(max)");

            // ✅ Relationships
            builder.HasMany(t => t.Questions)
                   .WithOne(q => q.Tour)
                   .HasForeignKey(q => q.TourId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.TourTickets)
                   .WithOne(tt => tt.Tour)
                   .HasForeignKey(tt => tt.TourId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.TourImages)
                   .WithOne(img => img.Tour)
                   .HasForeignKey(img => img.TourId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.TourCompany)
                   .WithMany(tc => tc.Tours)
                   .HasForeignKey(t => t.TourCompanyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}