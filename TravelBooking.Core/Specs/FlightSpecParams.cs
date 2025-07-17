using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Specs
{
    public class FlightSpecParams
    {
        private const int MaxPageSize = 10;

        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public int PageIndex { get; set; } = 1;
        public string? Sort { get; set; }

        public string? ArrivalAirport { get; set; }
        public DateTime? DepatureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
    }
}