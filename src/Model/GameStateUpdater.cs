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

        public static GameState MoveBall(this GameState gameState, DateTimeOffset TickTimestamp)
        {
            var previousBall = gameState.Ball;

            var elapsedTime =  DateTimeOffset.Now - TickTimestamp;
            
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
                var edgeStart = GetPlayerAreaEndCoords(player, gameState.Players.Count, End.Start);
                var edgeEnd = GetPlayerAreaEndCoords(player, gameState.Players.Count, End.End);
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
            var paddleStart = GetPlayerAreaEndCoords(player, numPlayers, End.Start);
            var paddleEnd = GetPlayerAreaEndCoords(player, numPlayers, End.End);
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
            var paddleLine = new Line(GetPaddleEndCoords(player, numPlayers, End.Start), GetPaddleEndCoords(player, numPlayers, End.End));
            var intersectionPoint = LineIntersectionService.FindIntersection(ballLine, paddleLine);
            var newVelocity = GetBallVelocityAfterCollision(oldBall, gameState, player);


            // who even knows what this will look like
            return new Ball(intersectionPoint, newVelocity);
        }

        public static Vector GetBallVelocityAfterCollision(Vector oldBall, GameState gameState, Player player)
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
            // Todo: increase ball's speed based on fewer players remaining

            var _random = new Random();
            var velocityMagnitude = 1;
            var randomAngle = _random.NextDouble()*2*Math.PI;
            var randomx = velocityMagnitude * Math.Cos(randomAngle);
            var randomy = velocityMagnitude * Math.Cos(randomAngle);
            
            var resurrectedBall = new Ball(Vector.Zero, new Vector(randomx, randomy));
            var newPlayerList = gameState.Players.Remove(playerToKill);
            return new GameState(resurrectedBall, newPlayerList);
        }

        //public static Vector GetPlayerAreaCentre(Player player, GameState gameState)
        //{
        //    // going with a game region radius of 1 for simplicity
        //    var numPlayers = gameState.Players.Count;
        //    var theta_n = 2*Math.PI*player.Index/numPlayers;
        //    var phi = Math.Cos(Math.PI/numPlayers);
        //    var xPos = phi*Math.Cos(theta_n);
        //    var yPos = phi*Math.Sin(theta_n); 
        //    return new Vector(xPos, yPos);
        //}

        public static Vector GetPlayerAreaEndCoords(Player player, int numPlayers, End end)
        {
            var adjustedIndex = end == End.Start ? player.Index - 1 : player.Index + 1;
            var alpha = (adjustedIndex)*Math.PI/numPlayers; // angle from the vertical of the relevant end
            return new Vector(Math.Cos(alpha), Math.Sin(alpha));
        }

        public static Vector GetPaddleEndCoords(Player player, int numPlayers, End end)
        {
            var vStart = GetPlayerAreaEndCoords(player, numPlayers, End.Start); // vector coordinates of start end of area
            var vEnd = GetPlayerAreaEndCoords(player, numPlayers, End.End); // vector coordinates of end end of area

            double proportionalPaddleLength = 0.3;
            var adjustedPosition = player.Position * (1 - proportionalPaddleLength)
                + proportionalPaddleLength / 2;

            var scaleBy = adjustedPosition;
            scaleBy += end == End.Start ? -1 * proportionalPaddleLength / 2 : proportionalPaddleLength / 2;

            return vStart.Add(vEnd.Subtract(vStart).ScalarMultiply(scaleBy));
        }
    }

    public enum End
    {
        Start,
        End
    }
}
