using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TravelBooking.Core.Configurations
{
    public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : Models.BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id); 
        }
    }
}