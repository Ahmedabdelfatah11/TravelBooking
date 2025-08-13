using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications.CarSpecs
{
    public class CarSpecifications : BaseSpecifications<Car>
    {
        public CarSpecifications(CarSpecParams carParams)
         : base(c =>
             (string.IsNullOrEmpty(carParams.Model) || c.Model.ToLower().Contains(carParams.Model.ToLower())) &&
             (string.IsNullOrEmpty(carParams.Location) || c.Location.ToLower().Contains(carParams.Location.ToLower())) &&
             (!carParams.MinPrice.HasValue || c.Price >= carParams.MinPrice.Value) &&
             (!carParams.MaxPrice.HasValue || c.Price <= carParams.MaxPrice.Value)
         )
        {
            if (!string.IsNullOrEmpty(carParams.Sort))
            {
                switch (carParams.Sort.ToLower())
                {
                    case "modelasc":
                        AddOrderBy(x => x.Model);
                        break;
                    case "modeldesc":
                        AddOrderByDesc(x => x.Model);
                        break;
                    case "priceasc":
                        AddOrderBy(x => x.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDesc(x => x.Price);
                        break;
                    default:
                        AddOrderBy(x => x.Model);
                        break;
                }
            }
            else
            {
                AddOrderBy(x => x.Model);
            }

            ApplyPagination((carParams.PageIndex - 1) * carParams.PageSize, carParams.PageSize);
        }

        public CarSpecifications(int id)
    : base(c => c.Id == id)
        {

        }
    }
}
