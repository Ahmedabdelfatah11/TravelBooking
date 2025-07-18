using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Specifications
{
    public class HotelCompanySpecParams: PaginationParams
    {
        public string? Search { get; set; }
        public string? Sort { get; set; }
    }
}
