namespace Web.Hubs;

using Microsoft.AspNetCore.SignalR;

public class ActivityHub : Hub
{
    public async Task NotifyActivityCreateAsync(string message)
    {
        await this.Clients.Others.SendAsync("ReceiveActivityCreate", message);
    }
}