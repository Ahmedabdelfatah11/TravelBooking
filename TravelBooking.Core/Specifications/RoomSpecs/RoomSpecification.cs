using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications.RoomSpecs
{
    public class RoomSpecification : BaseSpecifications<Room>
    {
        public RoomSpecification(RoomSpecParams specParams)
        : base(r =>
        (string.IsNullOrEmpty(specParams.Search) ||
         r.RoomType.ToString().ToLower().Contains(specParams.Search.ToLower())) &&
        (!specParams.IsAvailable.HasValue || r.IsAvailable == specParams.IsAvailable.Value))

        {
            Includes.Add(r => r.Images);

            AddOrderBy(r => r.Id); // Default

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort.ToLower())
                {
                    case "priceasc":
                        AddOrderBy(r => r.Price);
                        break;

                    case "pricedesc":
                        AddOrderByDesc(r => r.Price);
                        break;
                }
            }

            ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }

        public RoomSpecification(int id) : base(r => r.Id == id)
        {
            Includes.Add(r => r.Images);
        }
    }
}
