using System;
using Microsoft.Extensions.Hosting;
using Model;
using Web.Models;

namespace Web.Services
{
    public interface IGameEngineService : IHostedService
    {
        AddGameResult AddGame();
        void StartGame(string gameCode);
        AddPlayerResult AddPlayer(string gameCode, string name);
        GameState GetGameState(string gameCode);
        void MovePlayer(string gameCode, Guid playerId, float position);
        bool CanJoinGame(string gameCode);
    }
}
