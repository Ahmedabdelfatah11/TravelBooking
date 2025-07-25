using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications.TourCompanySpecs
{
    public class TourCompanyWithFilterationForCountSpecifications :BaseSpecifications<TourCompany>
    {
        public TourCompanyWithFilterationForCountSpecifications(TourCompanySpecParams specParams)
           : base(x =>
               (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
               (string.IsNullOrEmpty(specParams.Location) || x.Location.ToLower() == specParams.Location) &&
               (string.IsNullOrEmpty(specParams.Rating) || x.rating == specParams.Rating)
           )
        {

        }
    }
}
