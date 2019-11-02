using System;
using System.Collections.Generic;

namespace Model
{
    public class Player
    {
        public Guid Id { get; }
        public int Index { get; }
        public string Name { get; }
        public string Color { get; }
        public double Position { get; } // position of centre of paddle from 0 to 1
        public PlayerArea PlayerArea { get; set; }

        public Player(Guid id, int index, string name, string color, double position)
        {
            Id = id;
            Index = index;
            Name = name;
            Color = color;
            Position = position;
            PlayerArea = new PlayerArea(index);
        }

        public Vector GetPaddleStartCoords()
        {
            var vStart = PlayerArea.StartCoords; // vector coordinates of start end of area
            var vEnd = PlayerArea.EndCoords; // vector coordinates of end end of area

            double proportionalPaddleLength = 0.3; // will get from gamestate!!!!
            var adjustedPosition = Position * (1-proportionalPaddleLength) 
                + proportionalPaddleLength/2;

            return vStart.Add(
                vEnd.Subtract(vStart).ScalarMultiply(
                    (adjustedPosition - proportionalPaddleLength / 2) 
                )
            );
        }

        public Vector GetPaddleEndCoords()
        {
            var vStart = PlayerArea.StartCoords; // vector coordinates of start end of area
            var vEnd = PlayerArea.EndCoords; // vector coordinates of end end of area

            double proportionalPaddleLength = 0.3; // will get from gamestate!!!!
            var adjustedPosition = Position * (1-proportionalPaddleLength) 
                + proportionalPaddleLength/2;

            return vStart.Add(
                vEnd.Subtract(vStart).ScalarMultiply(
                    (adjustedPosition + proportionalPaddleLength / 2) 
                )
            );
        }
    }
}
