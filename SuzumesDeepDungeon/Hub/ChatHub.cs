namespace SuzumesDeepDungeon.Hub;

// Hubs/ChatHub.cs
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    // Метод для отправки сообщения всем клиентам
    public async Task SendMessage(string user, string message)
    {
        // ReceiveMessage - метод, который будет вызываться на клиенте
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    // Подключение нового пользователя
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    // Отключение пользователя
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}