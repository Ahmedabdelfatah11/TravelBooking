using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications.TourSpescs
{
    public class ToursWithTourCompanyAdminSpec : BaseSpecifications<Tour>
    {
        public ToursWithTourCompanyAdminSpec(string adminId)
            : base(t => t.TourCompany.AdminId == adminId)
        {
            AddInclude(t => t.TourCompany);
            AddInclude(t => t.TourImages);
        }
    }
}