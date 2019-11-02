using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;

namespace Model
{
    public class GameEngine
    {
        private readonly int _tickDelay;
        private readonly ReaderWriterLockSlim _moveLock = new ReaderWriterLockSlim();
        private readonly ConcurrentQueue<Move> _moves = new ConcurrentQueue<Move>();

        public GameEngine(int tickDelay)
        {
            _tickDelay = tickDelay;

            State = new GameState(
                new Ball(0, 0, Vector.Zero),
                Enumerable.Empty<Player>()
            );
        }

        public GameState State { get; private set; }

        public async Task Run(CancellationToken ct)
        {
            while (true)
            {
                var delay = Task.Delay(_tickDelay);
                var update = Tick();

                await Task.WhenAll(delay, update);
            }
        }

        public void MovePlayer(Guid playerId, float position)
        {
            var move = new Move(playerId, position);

            _moveLock.EnterReadLock();

            try
            {
                _moves.Enqueue(move);
            }
            finally
            {
                _moveLock.ExitReadLock();
            }
        }

        private async Task Tick()
        {
            _moveLock.EnterWriteLock();

            try
            {
                var updater = new GameStateUpdater(State);
                State = await updater.GetUpdatedGameState();
            }
            finally
            {
                _moveLock.ExitWriteLock();
            }
        }
    }
}