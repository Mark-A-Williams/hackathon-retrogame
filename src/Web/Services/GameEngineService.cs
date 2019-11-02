using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Model;
using Web.Models;

namespace Web.Services
{
    public class GameEngineService : BackgroundService
    {
        private readonly GameCodeService _codeService;

        public GameEngineService(GameCodeService codeService)
        {
            _codeService = codeService;
        }

        private readonly int _tickDelay = 300;

        private readonly ConcurrentDictionary<string, GameEngine> _games = new ConcurrentDictionary<string, GameEngine>();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var delay = Task.Delay(_tickDelay);

                foreach (var game in _games.Values)
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

        public AddPlayerResult AddPlayer(string gameCode, string name)
            => GetGameEngine(gameCode).AddPlayer(name);

        public GameState GetGameState(string gameCode)
            => GetGameEngine(gameCode).State;

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
