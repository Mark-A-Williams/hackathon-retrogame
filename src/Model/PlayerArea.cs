using System;

namespace Model
{
    public class PlayerArea
    {
        public int PlayerIndex { get; set; }
        public Vector CentreCoords { get; set;}
        
        // the coords (x,y) of the first end, going clockwise from the top of the game area
        public Vector StartCoords { get; set; }

        // and the second end
        public Vector EndCoords { get; set; }

        public PlayerArea(int index)
        {
            PlayerIndex = index;
            GetPlayerAreaCentreFromPlayerIndex();
            GetPlayerAreaEndsFromPlayerIndex();
        }

        public void GetPlayerAreaCentreFromPlayerIndex()
        {
            // going with a game region radius of 1 for simplicity
            var numPlayers = 8; // will get this from gamestate, could be any int!
            var theta_n = 2*Math.PI*PlayerIndex/numPlayers;
            var phi = Math.Cos(Math.PI/numPlayers);
            var xPos = phi*Math.Cos(theta_n);
            var yPos = phi*Math.Sin(theta_n); 
            CentreCoords = new Vector(xPos, yPos);
        }

        public void GetPlayerAreaEndsFromPlayerIndex()
        {
            var numPlayers = 8; // will get this from gamestate, could be any int!
            var alpha_1 = (PlayerIndex-1)*Math.PI/numPlayers; // angle from the vertical of the first end
            var alpha_2 = (PlayerIndex+1)*Math.PI/numPlayers;
            StartCoords = new Vector(Math.Cos(alpha_1), Math.Sin(alpha_1));
            EndCoords = new Vector(Math.Cos(alpha_2), Math.Sin(alpha_2));
        }
    }
}