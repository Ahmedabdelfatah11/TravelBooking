using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Params
{
    
        public class CarSpecParams
        {
            public int PageIndex { get; set; } = 1;
            public int PageSize { get; set; } = 10;

            public string? Model { get; set; }
            public decimal? MinPrice { get; set; }
            public decimal? MaxPrice { get; set; }
        }

    }
