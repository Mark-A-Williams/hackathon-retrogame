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

        public Vector GetCentreCoordsFromPlayerIndex(int index, int numPlayers)
        {
            var theta_n = 2*Math.PI*index/numPlayers;
            var phi = Math.Cos(Math.PI/numPlayers);
            var xPos = phi*Math.Cos(theta_n);
            var yPos = phi*Math.Sin(theta_n); 
            var centreCoord = new Vector(xPos, yPos);
            return centreCoord;
        }
    }
}
