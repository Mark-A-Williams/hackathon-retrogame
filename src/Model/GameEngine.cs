using System;
using System.Threading;
using System.Threading.Tasks;

namespace Model
{
    public class GameEngine
    {
        private readonly int _tickDelay;

        public GameEngine(int tickDelay)
        {
            _tickDelay = tickDelay;
        }

        public object Model { get; private set; }

        public async Task Run(CancellationToken ct)
        {
            while (true)
            {
                var delay = Task.Delay(_tickDelay);
                var update = Tick();

                await Task.WhenAll(delay, update);
            }
        }

        public void MovePlayer(Guid playerId, float position) {
        }

        private async Task Tick()
        {
            var updater = new ModelUpdater(Model);
            Model = updater.GetUpdatedModel();
        }
    }
}
