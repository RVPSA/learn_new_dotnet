using ChatAppWithSignalR.DataService;
using ChatAppWithSignalR.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppWithSignalR.Hubs;

public class ChatHub(ShareDb shareDb) : Hub
{
    private readonly ShareDb _shareDb = shareDb;


    public async Task JoinChat(UserConnection conn)
    {
        await Clients.All.SendAsync("ReceiveMessage", "admin", $"{conn.UserName} has joined");
    }

    public async Task JoinSpecificChatRoom(UserConnection conn)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId,conn.ChatRoom);
        
        _shareDb.Connections[Context.ConnectionId] = conn;
        await Clients.Group(conn.ChatRoom).SendAsync("ReceiveMessage", "admin", $"{conn.UserName} has joined {conn.ChatRoom}");
        
    }

    public async Task SendMessage(string message)
    {
        if (_shareDb.Connections.TryGetValue(Context.ConnectionId, out var connection))
        {
            await Clients.Group(connection.ChatRoom).SendAsync("ReceiveSpecificMessage",connection.UserName,message);
        }
    }
}