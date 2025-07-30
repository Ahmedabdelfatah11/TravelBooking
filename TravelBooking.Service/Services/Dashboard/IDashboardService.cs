using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Service.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<object> GetSuperAdminDashboardAsync();
    }
}
