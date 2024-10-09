using Microsoft.AspNetCore.SignalR;

namespace Foodies.Hubs
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.User(user).SendAsync("ReceiveMessage", user, message);
        }

    }
}
