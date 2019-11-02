using System;
using System.Collections.Generic;

namespace Model
{
    public class Player
    {
        public int Index { get; }
        public string Color { get; }
        public float Position { get; } // position of centre of paddle from 0 to 1

        public Player(int index, string color, float position)
        {
            Index = index;
            Color = color;
            Position = position;
        }

        public Vector GetCentreCoordsFromPlayerIndex(int index)
        {
            // index: 0 to N-1 (N players)
            var numPlayers = 8; // will get this from gamestate, could be any int!
            var theta_n = 2*Math.PI*index/numPlayers;
            var phi = Math.Cos(Math.PI/numPlayers);
            var xPos = phi*Math.Cos(theta_n);
            var yPos = phi*Math.Sin(theta_n); 
            var centreCoords = new Vector(xPos, yPos);
            return centreCoords;
        }

        public List<Vector> GetEndCoordsFromPlayerPosition(double playerPosition)
        {
            // this method might live somewhere else eventually hence some slightly redundant code
            
            // playerposition: 0+paddleLength/2 to 1-paddleLength/2

            var numPlayers = 8; // will get this from gamestate, could be any int!
            var paddleLength = 0.3; // will DEFINITELY get from gamestate, but probs about this
            var centreCoords = GetCentreCoordsFromPlayerIndex(Index);            

            var end_1 = new Vector(0,0);
            var end_2 = new Vector(0,0);
            var ends = new List<Vector>{end_1, end_2};
            return ends;
        }
    }
}
