using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications.HotelCompanySpecs
{
    public class HotelCompanyWithFiltersForCountSpecification : BaseSpecifications<HotelCompany>
    {
        public HotelCompanyWithFiltersForCountSpecification(HotelCompanySpecParams specParams)
       : base(h =>
           string.IsNullOrEmpty(specParams.Search) ||
           h.Name.ToLower().Contains(specParams.Search.ToLower()) ||
           h.Location.ToLower().Contains(specParams.Search.ToLower()) ||
           h.Description.ToLower().Contains(specParams.Search.ToLower()))
        {
        }
    }
}
