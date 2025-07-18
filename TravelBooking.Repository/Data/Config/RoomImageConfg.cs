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
    public class RoomImageConfg : BaseEntityConfiguration<RoomImage>
    {
        public override void Configure(EntityTypeBuilder<RoomImage> builder)
        {

            builder
                          .HasOne(img => img.Room)
                          .WithMany(r => r.Images)
                          .HasForeignKey(img => img.RoomId);

            // You can add more configuration here (e.g. max length for ImageUrl)
            builder.Property(p => p.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
