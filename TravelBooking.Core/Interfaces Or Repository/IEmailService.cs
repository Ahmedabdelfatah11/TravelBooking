using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Interfaces_Or_Repository
{
    public interface IEmailService
    {
        Task SendContactNotificationAsync(ContactMessage message, string userEmail);
    } 
}
