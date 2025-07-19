using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Repository.Data.Config
{
    public class TourImageConfig : IEntityTypeConfiguration<TourImage>
    {
        public void Configure(EntityTypeBuilder<TourImage> builder)
        {
           builder.HasOne(ti => ti.Tour)
                  .WithMany(t => t.TourImages)
                  .HasForeignKey(ti => ti.TourId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
