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

        public static GameState ApplyCollisionDetection(this GameState gameState, Vector oldBall)
        {
            var newBall = gameState.Ball.Position;
            var playerIntersectedIndex = DidBallMoveIntersectPlayAreaEdge(oldBall, newBall);
            if (playerIntersectedIndex == null)
            {
                return gameState;
            }
            else
            {
                var playerDeflectedBall = DidBallMoveIntersectPlayerPaddle(oldBall, newBall, playerIntersectedIndex.Value);
                if (playerDeflectedBall)
                {
                    var deflectedBall = CalculateBallCollision();
                    return new GameState(
                        deflectedBall,
                        gameState.Players
                    );
                }
                else
                {
                    return KillPlayer(gameState, playerIntersectedIndex.Value);
                }
            }
        }

        public static int? DidBallMoveIntersectPlayAreaEdge(Vector oldBall, Vector newBall)
        {
            return 3;
        }

        public static bool DidBallMoveIntersectPlayerPaddle(Vector oldBall, Vector newBall, int playerIndex)
        {
            return false;
        }

        public static Ball CalculateBallCollision()
        {
            // who even knows what this will look like
            return new Ball(Vector.Zero, Vector.Zero);
        }

        public static GameState KillPlayer(GameState gameState, int playerIndex)
        {
            // Todo (at least the ball's velocity, rest might be ok)
            var resurrectedBall = new Ball(Vector.Zero, Vector.Zero);
            var playerToKill = gameState.Players.Where(p => p.Index == playerIndex).FirstOrDefault();
            var newPlayerList = gameState.Players.Remove(playerToKill);
            return new GameState(resurrectedBall, newPlayerList);
        }
    }
}
