using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Core.Specifications;

namespace TravelBooking.Core.Specifications.FlightSpecs
{
    public class FlightCountSpec : BaseSpecifications<Flight>
    {
        public FlightCountSpec(FlightSpecParams specParams) : base(f =>
            (string.IsNullOrEmpty(specParams.DepartureAirport) || f.DepartureAirport.ToString().ToLower().Contains(specParams.DepartureAirport)) &&
            (string.IsNullOrEmpty(specParams.ArrivalAirport) || f.ArrivalAirport.ToString().ToLower().Contains(specParams.ArrivalAirport)) &&
            (!specParams.DepartureTime.HasValue || f.DepartureTime.Date == specParams.DepartureTime.Value.Date) &&
            (!specParams.ArrivalTime.HasValue || f.ArrivalTime.Date == specParams.ArrivalTime.Value.Date))
        {
        }

    }
}
