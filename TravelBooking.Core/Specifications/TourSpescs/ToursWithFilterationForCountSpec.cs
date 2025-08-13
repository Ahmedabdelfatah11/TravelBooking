using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications.TourSpecs
{
    public class ToursWithFilterationForCountSpec : BaseSpecifications<Tour>
    {
        public ToursWithFilterationForCountSpec(TourSpecParams specParams)
        :base(x =>
             (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
                (string.IsNullOrEmpty(specParams.Destination) || x.Destination.ToLower() == specParams.Destination) &&
                (specParams.Category == null || specParams.Category.Count == 0 || specParams.Category.Contains(x.Category.Value)) &&
            (!specParams.MinPrice.HasValue || x.Price >= specParams.MinPrice.Value) &&
            (!specParams.MaxPrice.HasValue || x.Price <= specParams.MaxPrice.Value)

        )
        {

        }
    }
}
