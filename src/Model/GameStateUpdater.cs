using System;
using System.Linq;
using System.Collections.Concurrent;

namespace Model
{
    internal static class GameStateUpdater
    {
        public static GameState ApplyMoves(this GameState gameState, ConcurrentQueue<Move> moves)
        {
            var players = gameState.Players.ToDictionary(o => o.Id, o => o);

            foreach (var playerId in moves.Select(o => o.PlayerId).Distinct())
            {
                if (players.ContainsKey(playerId))
                {
                    var lastMove = moves.Last(o => o.PlayerId == playerId);
                    players[playerId] = players[playerId].WithPosition(lastMove.Position);
                }
            }

            return new GameState(
                gameState.Ball,
                players.Values.OrderBy(o => o.Index)
            );
        }

        public static GameState MoveBall(this GameState gameState)
        {
            var previousBall = gameState.Ball;

            var elapsedTime = DateTimeOffset.Now - gameState.TickTimestamp;
            
            var newPosition = previousBall.Position.Add(
                previousBall.Velocity.ScalarMultiply(elapsedTime.TotalSeconds)
            );

            var newBall = new Ball(newPosition, previousBall.Velocity);

            return new GameState(
                newBall,
                gameState.Players
            );
        }

        public static GameState ApplyCollisionDetection(this GameState gameState)
        {
            return gameState;
        }
    }
}
