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
            TickTimestamp = DateTimeOffset.Now;
        }

        public DateTimeOffset TickTimestamp { get; }
        public Ball Ball { get; }
        public ImmutableList<Player> Players { get; }
    }
}
