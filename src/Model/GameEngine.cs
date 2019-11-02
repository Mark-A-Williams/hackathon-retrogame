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

        public async Task Run(CancellationToken ct)
        {
            while (true)
            {
                var delay = Task.Delay(_tickDelay);
                var update = Tick();

                await Task.WhenAll(delay, update);
            }
        }

        private async Task Tick()
        {
            // TODO update game model.
        }
    }
}
