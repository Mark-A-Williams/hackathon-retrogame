using System;
using Model;

public interface IPlayerStore
{
    public void AddPlayer(string connectionId, PlayerDetails player);

    public PlayerDetails GetPlayer(string connectionId);

    public bool PlayerExists(string connectionId);
}