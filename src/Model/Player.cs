using System;

namespace Model
{
    public class Player
    {
        public int Index { get; }
        public string Color { get; }
        public float Position { get; }

        public Player(int index, string color, float position)
        {
            Index = index;
            Color = color;
            Position = position;
        }

        public Vector GetCentreCoordsFromPlayerIndex(int index)
        {
            // var N = GameState.Players
            // var xPos = Math.Cos(Math.PI/)
            var centreCoord = new Vector(0,0);
            return centreCoord
        }
    }
}
