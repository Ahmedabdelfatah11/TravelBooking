using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Repository.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinGroup(string sessionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        }

        public async Task LeaveGroup(string sessionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId);
        }

        public async Task SendMessage(string sessionId, string message)
        {
            await Clients.Group(sessionId).SendAsync("ReceiveMessage", new
            {
                message,
                timestamp = DateTime.UtcNow,
                isFromUser = true
            });
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
