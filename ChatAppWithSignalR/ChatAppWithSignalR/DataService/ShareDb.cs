using System.Collections.Concurrent;
using ChatAppWithSignalR.Models;

namespace ChatAppWithSignalR.DataService;

public class ShareDb
{
    private readonly ConcurrentDictionary<string, UserConnection> _connections = new();

    public ConcurrentDictionary<string, UserConnection> Connections => _connections;
}