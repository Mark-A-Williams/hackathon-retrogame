using System;
using System.Collections.Concurrent;

public class PlayerStore : IPlayerStore
{
    private ConcurrentDictionary<string, PlayerDetails> _players;

    public PlayerStore()
    {
        _players = new ConcurrentDictionary<string, PlayerDetails>();
    }

    public void AddPlayer(string connectionId, PlayerDetails player)
    {
        _players.AddOrUpdate(connectionId, player, (key, old) => player);
    }

    public PlayerDetails GetPlayer(string connectionId)
    {
        var playerExists = _players.TryGetValue(connectionId, out var player);

        return playerExists ? player : throw new InvalidOperationException($"Player {connectionId} does not exist.");
    }

    public bool PlayerExists(string connectionId)
    {
        return _players.ContainsKey(connectionId);
    }
}