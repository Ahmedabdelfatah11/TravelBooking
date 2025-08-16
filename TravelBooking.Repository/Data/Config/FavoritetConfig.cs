using Microsoft.EntityFrameworkCore;
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
    public class FavoritetConfig : BaseEntityConfiguration<Favoritet>
    {
        public override void Configure(EntityTypeBuilder<Favoritet> builder)
        {
            base.Configure(builder);

            builder.HasOne(f => f.User)
                            .WithMany(f => f.favoritets)
                            .HasForeignKey(f => f.UserId)
                            .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(f => f.HotelCompany)
                .WithMany(f => f.favoritets)
                .HasForeignKey(f => f.HotelCompanyId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.HasOne(f => f.TourCompany)
                  .WithMany(f => f.favoritets)
                  .HasForeignKey(f => f.TourCompanyId)
                  .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(f => new { f.UserId, f.HotelCompanyId, f.TourCompanyId }).IsUnique();
            builder.HasIndex(f => new { f.UserId, f.TourId }).IsUnique();

        }
    }
}