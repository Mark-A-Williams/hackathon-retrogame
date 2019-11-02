using System;
using System.Collections.Concurrent;

namespace Model
{
    internal static class GameStateUpdater
    {
        public static GameState ApplyMoves(this GameState gameState, ConcurrentQueue<Move> moves)
        {
            return gameState;
        }

        public static GameState ApplyCollisionDetection(this GameState gameState)
        {
            return gameState;
        }
    }
}
