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
            var playerIntersectedIndex = DidBallMoveIntersectPlayAreaEdge(gameState, oldBall, newBall);
            if (playerIntersectedIndex == null)
            {
                return gameState;
            }
            else
            {
                var player = gameState.Players.Where(p => p.Index == playerIntersectedIndex).FirstOrDefault();
                var playerDeflectedBall = DidBallMoveIntersectPlayerPaddle(oldBall, newBall, player, gameState.Players.Count);
                if (playerDeflectedBall)
                {
                    var deflectedBall = CalculateBallCollision(oldBall, newBall, player, gameState);
                    return new GameState(
                        deflectedBall,
                        gameState.Players
                    );
                }
                else
                {
                    return KillPlayer(gameState, player);
                }
            }
        }

        public static int? DidBallMoveIntersectPlayAreaEdge(GameState gameState, Vector oldBall, Vector newBall)
        {
            foreach (var player in gameState.Players)
            {
                var edgeStart = GetPlayerAreaStart(player, gameState.Players.Count);
                var edgeEnd = GetPlayerAreaEnd(player, gameState.Players.Count);
                var ballEncounteredPlayersArea = LinesIntersect(
                    edgeStart, edgeEnd, oldBall, newBall
                );
                if (ballEncounteredPlayersArea) {
                    return player.Index;
                }
            }
            return null;
        }

        public static bool DidBallMoveIntersectPlayerPaddle(Vector oldBall, Vector newBall, Player player, int numPlayers)
        {
            var paddleStart = GetPaddleStartCoords(player, numPlayers);
            var paddleEnd = GetPaddleEndCoords(player, numPlayers);
            return LinesIntersect(
                    paddleStart, paddleEnd, oldBall, newBall
                );

        }

        public static bool OnSegment(Vector p, Vector q, Vector r) 
        { 
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) 
            && q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
            {
                return true;
            }
             
            return false; 
        } 
  
        public static double Orientation(Vector p, Vector q, Vector r) 
        { 
            double val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y); 
  
            if (val == 0) return 0; // colinear 
  
            return (val > 0)? 1: 2; // clock or counterclock wise 
        } 

        public static bool LinesIntersect(Vector firstStart, Vector firstEnd, Vector secondStart, Vector secondEnd)
        {
            double o1 = Orientation(firstStart, firstEnd, secondStart); 
            double o2 = Orientation(firstStart, firstEnd, secondEnd); 
            double o3 = Orientation(secondStart, secondEnd, firstStart); 
            double o4 = Orientation(secondStart, secondEnd, firstEnd); 
  
            // General case 
            if (o1 != o2 && o3 != o4) return true; 
  
            if (o1 == 0 && OnSegment(firstStart, secondStart, firstEnd)) return true; 
            if (o2 == 0 && OnSegment(firstStart, secondEnd, firstEnd)) return true; 
            if (o3 == 0 && OnSegment(secondStart, firstStart, secondEnd)) return true; 
            if (o4 == 0 && OnSegment(secondStart, firstEnd, secondEnd)) return true; 
  
            return false;
        }

        public static Ball CalculateBallCollision(Vector oldBall, Vector newBall, Player player, GameState gameState)
        {
            var numPlayers = gameState.Players.Count;
            var ballLine = new Line(oldBall, newBall);
            var paddleLine = new Line(GetPaddleStartCoords(player, numPlayers), GetPaddleEndCoords(player, numPlayers));
            var intersectionPoint = LineIntersectionService.FindIntersection(ballLine, paddleLine);
            var newVelocity = BallVelocityAfterCollision(oldBall, gameState, player);


            // who even knows what this will look like
            return new Ball(intersectionPoint, newVelocity);
        }

        public static Vector BallVelocityAfterCollision(Vector oldBall, GameState gameState, Player player)
        {
            var velocityAbsolute = Math.Sqrt(Math.Pow(oldBall.X,2)+Math.Pow(oldBall.Y,2));
            var beta = Math.PI*(1 - 2*player.Index/gameState.Players.Count);
            var initialAngle = Math.Atan(oldBall.Y/oldBall.X);
            var finalAngle = 2*beta + 2*Math.PI - initialAngle;
            var xVelocity = velocityAbsolute * Math.Cos(finalAngle);
            var yVelocity = velocityAbsolute * Math.Sin(finalAngle);
            return new Vector(xVelocity, yVelocity);
        }

        public static GameState KillPlayer(GameState gameState, Player playerToKill)
        {
            // Todo (at least the ball's velocity, rest might be ok)
            var resurrectedBall = new Ball(Vector.Zero, Vector.Zero);
            var newPlayerList = gameState.Players.Remove(playerToKill);
            return new GameState(resurrectedBall, newPlayerList);
        }

        public static Vector GetPlayerAreaCentre(Player player, GameState gameState)
        {
            // going with a game region radius of 1 for simplicity
            var numPlayers = 8; // will get this from gamestate, could be any int!
            var theta_n = 2*Math.PI*player.Index/numPlayers;
            var phi = Math.Cos(Math.PI/numPlayers);
            var xPos = phi*Math.Cos(theta_n);
            var yPos = phi*Math.Sin(theta_n); 
            return new Vector(xPos, yPos);
        }

        public static Vector GetPlayerAreaStart(Player player, int numPlayers)
        {
            var alpha_1 = (player.Index-1)*Math.PI/numPlayers; // angle from the vertical of the start
            return new Vector(Math.Cos(alpha_1), Math.Sin(alpha_1));
        }

        public static Vector GetPlayerAreaEnd(Player player, int numPlayers)
        {
            var alpha_2 = (player.Index+1)*Math.PI/numPlayers; // angle from the vertical of the end
            return new Vector(Math.Cos(alpha_2), Math.Sin(alpha_2));
        }

        public static Vector GetPaddleStartCoords(Player player, int numPlayers)
        {
            var vStart = GetPlayerAreaStart(player, numPlayers); // vector coordinates of start end of area
            var vEnd = GetPlayerAreaEnd(player, numPlayers); // vector coordinates of end end of area

            double proportionalPaddleLength = 0.3; // will get from gamestate!!!!
            var adjustedPosition = player.Position * (1-proportionalPaddleLength) 
                + proportionalPaddleLength/2;

            return vStart.Add(
                vEnd.Subtract(vStart).ScalarMultiply(
                    (adjustedPosition - proportionalPaddleLength / 2) 
                )
            );
        }

        // this method is literally a duplicate of the previous one but with a sign change
        public static Vector GetPaddleEndCoords(Player player, int numPlayers)
        {
            var vStart = GetPlayerAreaStart(player, numPlayers); // vector coordinates of start end of area
            var vEnd = GetPlayerAreaEnd(player, numPlayers); // vector coordinates of end end of area

            double proportionalPaddleLength = 0.3; // will get from gamestate!!!!
            var adjustedPosition = player.Position * (1-proportionalPaddleLength) 
                + proportionalPaddleLength/2;

            return vStart.Add(
                vEnd.Subtract(vStart).ScalarMultiply(
                    (adjustedPosition + proportionalPaddleLength / 2) 
                )
            );
        }
    }
}
