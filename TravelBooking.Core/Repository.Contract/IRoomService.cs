using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Repository.Contract
{
    public record DateRange(DateTime Start, DateTime End);
    public interface IRoomService
    {
         Task<List<DateRange>> GetAvailableDateRanges(int roomId, DateTime start, DateTime end);
    }
}
