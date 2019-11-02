using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Model;
using Web.Models;

namespace Web.Services
{
    public class GameEngineService : BackgroundService, IGameEngineService
    {
        private readonly IGameCodeService _codeService;

        public GameEngineService(IGameCodeService codeService)
        {
            _codeService = codeService;
        }

        private readonly int _tickDelayActive = 100;
        private readonly int _tickDelayIdle = 3000;

        private readonly ConcurrentDictionary<string, GameEngine> _games = new ConcurrentDictionary<string, GameEngine>();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var activeGames = _games.Values.Where(o => o.HasStarted).ToArray();

                var delay = Task.Delay(activeGames.Any() ? _tickDelayActive : _tickDelayIdle);

                foreach (var game in activeGames)
                {
                    game.Tick();
                }

                await delay;
            }
        }

        public AddGameResult AddGame()
        {
            var code = _codeService.GetCode();

            _games.TryAdd(code, new GameEngine());

            return new AddGameResult(code);
        }

        public void StartGame(string gameCode)
            => GetGameEngine(gameCode).Start();

        public AddPlayerResult AddPlayer(string gameCode, string name)
            => GetGameEngine(gameCode).AddPlayer(name);

        public GameState GetGameState(string gameCode)
            => GetGameEngine(gameCode).State;

        public void MovePlayer(string gameCode, Guid playerId, float position)
            => GetGameEngine(gameCode).MovePlayer(playerId, position);

        public bool CanJoinGame(string gameCode)
            => !GetGameEngine(gameCode).HasStarted;

        private GameEngine GetGameEngine(string code)
        {
            if (!_games.TryGetValue(code, out GameEngine game))
            {
                throw new ArgumentException($"Game code '{code}' not found.");
            }

            return game;
        }
    }
}
