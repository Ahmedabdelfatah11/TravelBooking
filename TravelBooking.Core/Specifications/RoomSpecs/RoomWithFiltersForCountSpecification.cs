using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications.RoomSpecs
{
    public class RoomWithFiltersForCountSpecification : BaseSpecifications<Room>
    {

        public RoomWithFiltersForCountSpecification(RoomSpecParams specParams)
          : base(r =>
        (string.IsNullOrEmpty(specParams.Search) || r.Description.ToLower().Contains(specParams.Search.ToLower())) &&
        (!specParams.RoomType.HasValue || r.RoomType == specParams.RoomType.Value) &&
        (!specParams.IsAvailable.HasValue || r.IsAvailable == specParams.IsAvailable.Value))
        {

        }

    }
}
