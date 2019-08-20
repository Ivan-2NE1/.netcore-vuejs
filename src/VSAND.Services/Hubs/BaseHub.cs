using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace VSAND.Services.Hubs
{
    public class BaseHub : Hub
    {
        public Task JoinRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }
    }
}
