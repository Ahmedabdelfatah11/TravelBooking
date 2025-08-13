using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications.TourSpecs
{
    public class ToursSpecification:BaseSpecifications<Tour>
    {
        public ToursSpecification(TourSpecParams specParams)
       : base(x =>
            (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
               (string.IsNullOrEmpty(specParams.Destination) || x.Destination.ToLower() == specParams.Destination) &&
             (specParams.Category == null || specParams.Category.Count == 0 || specParams.Category.Contains(x.Category.Value)) &&
           (!specParams.MinPrice.HasValue || x.Price >= specParams.MinPrice.Value) &&
           (!specParams.MaxPrice.HasValue || x.Price <= specParams.MaxPrice.Value)

       )
        {
            Includes.Add(x => x.TourCompany);
            Includes.Add(x => x.TourImages);
            Includes.Add(x => x.TourTickets);
            Includes.Add(x => x.Questions);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort.ToLower())
                {
                    case "nameasc":
                        AddOrderBy(x => x.Name);
                        break;
                    case "namedesc":
                        AddOrderByDesc(x => x.Name);
                        break;
                    case "priceasc":
                        AddOrderBy(x => x.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDesc(x => x.Price);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(x => x.Name);
            }

            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        
        }
        public ToursSpecification(int id)
      : base(t => t.Id == id)
        {
            Includes.Add(t => t.TourCompany);
            Includes.Add(t => t.TourImages);
            Includes.Add(t => t.TourTickets);
            Includes.Add(x => x.Questions);

        }
    }
}
