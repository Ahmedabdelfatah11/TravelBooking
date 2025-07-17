using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Core.Specifications;

namespace TravelBooking.Core.Specs
{
    public class FlightWithCompanySpecs : BaseSpecifications<FlightCompany>
    {
        public FlightWithCompanySpecs() 
        {
            Includes.Add(F => F.Flights);
        }
        public FlightWithCompanySpecs(int id):base(F=>F.Id == id)
        {
            Includes.Add(F => F.Flights);
        }
    }
}
