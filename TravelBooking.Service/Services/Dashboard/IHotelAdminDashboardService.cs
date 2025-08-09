using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Service.Services.Dashboard
{
    public interface IHotelAdminDashboardService
    {
        Task<object> GetStatsForHotel(int hotelId);
    }
}
