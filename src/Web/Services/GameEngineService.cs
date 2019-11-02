using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Model;

namespace Web.Services
{
    internal class GameEngineService : BackgroundService
    {
        private readonly int _tickDelay = 300;

        private readonly ConcurrentDictionary<string, GameEngine> _games = new ConcurrentDictionary<string, GameEngine>();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var delay = Task.Delay(_tickDelay);

                foreach (var game in _games.Values) {
                    game.Tick();
                }

                await delay;
            }
        }
    }
}
