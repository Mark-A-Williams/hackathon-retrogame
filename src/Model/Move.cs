using System;

namespace Model
{
    internal class Move
    {
        public Guid PlayerId { get; }
        public double Position { get; }

        public Move(Guid playerId, double position)
        {
            PlayerId = playerId;
            Position = position;
        }
    }
}
