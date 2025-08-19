using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications.TourCompanySpecs
{
    public class TourCompanyWithAdminSpec : BaseSpecifications<TourCompany>
    {
        public TourCompanyWithAdminSpec(string adminId)
            : base(tc => tc.AdminId == adminId)
        {
        }
    }
}
