using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications
{

    public class SpecificationWithCars : BaseSpecifications<CarRentalCompany>
    {
        public SpecificationWithCars(int pageIndex, int pageSize)
            : base()
        {
            AddInclude(x => x.Cars);
            ApplyPagination((pageIndex - 1) * pageSize, pageSize);
        }

        public SpecificationWithCars(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.Cars);
        }
    }

}
