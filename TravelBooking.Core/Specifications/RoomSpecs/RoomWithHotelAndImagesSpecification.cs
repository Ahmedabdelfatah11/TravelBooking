using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications.RoomSpecs
{
    public class RoomWithHotelAndImagesSpecification : BaseSpecifications<Room>
    {
        // Get All Rooms with Hotel + Images
        public RoomWithHotelAndImagesSpecification()
        {
            Includes.Add(r => r.Hotel!);
            Includes.Add(r => r.Images);
        }

        // Get Room By Id with Hotel + Images
        public RoomWithHotelAndImagesSpecification(int id)
            : base(r => r.Id == id)
        {
            Includes.Add(r => r.Hotel!);
            Includes.Add(r => r.Images);
        }

        // Filter by RoomType and Price
        public RoomWithHotelAndImagesSpecification(string? roomType, decimal? price)
            : base(r =>
                (string.IsNullOrEmpty(roomType) || r.RoomType.ToString().ToLower().Contains(roomType.ToLower())) &&
                (!price.HasValue || r.Price <= price)
            )
        {
            Includes.Add(r => r.Hotel!);
            Includes.Add(r => r.Images);
        }
    }
}

