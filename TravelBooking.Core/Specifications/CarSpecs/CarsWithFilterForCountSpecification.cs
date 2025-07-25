using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications
{
    public class CarsWithFilterForCountSpecification : BaseSpecifications<Car>
    {
        public CarsWithFilterForCountSpecification(CarSpecParams carParams)
            : base(c =>
                (string.IsNullOrEmpty(carParams.Model) || c.Model.ToLower().Contains(carParams.Model.ToLower())) &&
                (!carParams.MinPrice.HasValue || c.Price >= carParams.MinPrice.Value) &&
                (!carParams.MaxPrice.HasValue || c.Price <= carParams.MaxPrice.Value)
            )
        {
            Includes.Add(c => c.RentalCompany);
            ApplyPagination((carParams.PageIndex - 1) * carParams.PageSize, carParams.PageSize);

        }
      
    }

}
