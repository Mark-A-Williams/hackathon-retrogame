using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Model
{
    public class GameState
    {
        public GameState(
            Ball ball,
            IEnumerable<Player> players
        )
        {
            Ball = ball;
            Players = ImmutableList.CreateRange(players);
        }

        public Ball Ball { get; }
        public ImmutableList<Player> Players { get; }
    }
}
