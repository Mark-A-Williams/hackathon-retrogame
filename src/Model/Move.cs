using System;

namespace Model
{
    internal class Move
    {
        public Guid PlayerId { get; }
        public float Position { get; }

        public Move(Guid playerId, float position)
        {
            PlayerId = playerId;
            Position = position;
        }
    }
}