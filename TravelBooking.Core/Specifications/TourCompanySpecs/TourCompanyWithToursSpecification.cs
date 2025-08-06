using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Core.Specifications;

namespace TravelBooking.Repository.TourCompanySpecs
{
    public class TourCompanyWithToursSpecification : BaseSpecifications<TourCompany>
    {
        public TourCompanyWithToursSpecification(TourCompanySpecParams specParams)
            : base(x =>
                (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
                (string.IsNullOrEmpty(specParams.Location) || x.Location.ToLower() == specParams.Location) &&
                      (!specParams.Rating.HasValue || x.Rating == specParams.Rating))
        {
            Includes.Add(x => x.Tours);
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
                    case "ratingasc":
                        AddOrderBy(x => x.Rating);
                        break;
                    case "ratingdesc":
                        AddOrderByDesc(x => x.Rating);
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
        public TourCompanyWithToursSpecification(int id)
           : base(tc => tc.Id == id)
        {
            Includes.Add(tc => tc.Tours);
        }
    }

}
