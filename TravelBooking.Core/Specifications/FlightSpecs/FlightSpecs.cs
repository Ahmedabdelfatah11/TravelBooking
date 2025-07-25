using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Core.Specifications;

namespace TravelBooking.Core.Specifications.FlightSpecs
{
    public class FlightSpecs : BaseSpecifications<Flight>
    {
        public FlightSpecs(FlightSpecParams specParams) : base(f =>
            (string.IsNullOrEmpty(specParams.DepartureAirport) || f.DepartureAirport == specParams.DepartureAirport) &&
            (string.IsNullOrEmpty(specParams.ArrivalAirport) || f.ArrivalAirport == specParams.ArrivalAirport) &&
            (!specParams.DepatureTime.HasValue || f.DepartureTime.Date == specParams.DepatureTime.Value.Date) &&
            (!specParams.ArrivalTime.HasValue || f.ArrivalTime.Date == specParams.ArrivalTime.Value.Date))
        {
            Includes.Add(f => f.FlightCompany);
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        //OrderBy = P => P.Price;
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.FlightCompany);
                        break;
                }
            }
            ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }
        public FlightSpecs(int id) : base(f => f.Id == id)
        {
            Includes.Add(f => f.FlightCompany);
        }
    }
}
