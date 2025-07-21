using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications.CarRentalCompanySpecs
{

    public class CarRentalCompanySpecifications : BaseSpecifications<CarRentalCompany>
    {
        public CarRentalCompanySpecifications(CarRentalSpecParams specParams)
         : base(x =>
             (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
             (string.IsNullOrEmpty(specParams.Location) || x.Location.ToLower() == specParams.Location) &&
             (string.IsNullOrEmpty(specParams.Rating) || x.Rating == specParams.Rating)
         )
        {

            Includes.Add(x => x.Cars);
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


        public CarRentalCompanySpecifications(int id)
            : base(x => x.Id == id)
        {
            Includes.Add(x => x.Cars);
        }
    }

}
