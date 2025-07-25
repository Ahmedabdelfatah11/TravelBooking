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
            (string.IsNullOrEmpty(specParams.DepartureAirport) || f.DepartureAirport == specParams.DepartureAirport) &&
            (string.IsNullOrEmpty(specParams.ArrivalAirport) || f.ArrivalAirport == specParams.ArrivalAirport) &&
            (!specParams.DepatureTime.HasValue || f.DepartureTime.Date == specParams.DepatureTime.Value.Date) &&
            (!specParams.ArrivalTime.HasValue || f.ArrivalTime.Date == specParams.ArrivalTime.Value.Date))
        {
        }

    }
}
