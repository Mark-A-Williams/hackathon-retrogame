using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Linq;

namespace Model
{
    public class GameEngine : IDisposable
    {
        private readonly ReaderWriterLockSlim _moveLock = new ReaderWriterLockSlim();
        private readonly ConcurrentQueue<Move> _moves = new ConcurrentQueue<Move>();
        private bool _disposed = false;
        private Random _random;

        public GameEngine()
        {
            State = new GameState(
                new Ball(Vector.Zero, Vector.Zero),
                Enumerable.Empty<Player>()
            );

            _random = new Random();
        }

        public GameState State { get; private set; }

        public bool HasStarted { get; private set; }

        public bool IsActive { get; }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        public void Start()
        {
            if (!CanStart())
            {
                return;
            }

            // Flick
            var randomx = (_random.NextDouble() * 2) + 1;
            var randomy = (_random.NextDouble() * 2) + 1;

            var ball = new Ball(Vector.Zero, new Vector(randomx, randomy));

            State = new GameState(
                ball,
                State.Players
            );

            HasStarted = true;
        }

        public bool CanStart()
        {
            return !HasStarted && State.Players.Count >= 2;
        }

        public AddPlayerResult AddPlayer(string name)
        {
            _moveLock.EnterWriteLock();

            Player player;

            try
            {
                player = new Player(
                    Guid.NewGuid(),
                    State.Players.Count,
                    name,
                    "#FF00FF",
                    0.5
                );

                State = new GameState(
                    State.Ball,
                    Enumerable.Concat(
                        State.Players,
                        Enumerable.Repeat(player, 1)
                    )
                );
            }
            finally
            {
                _moveLock.ExitWriteLock();
            }

            return new AddPlayerResult(
                player.Id,
                player.Color
            );
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

        public void Tick()
        {
            _moveLock.EnterWriteLock();
            var oldBallPosition = State.Ball.Position;

            try
            {
                State = State
                    .ApplyMoves(_moves)
                    .MoveBall()
                    .ApplyCollisionDetection(oldBallPosition);
            }
            finally
            {
                _moveLock.ExitWriteLock();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _moveLock.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                State = null;
                _disposed = true;
            }
        }
    }
}
