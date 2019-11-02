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
        public List<Vector> PaddleEndCoords {get; set; }

        public Player(Guid id, int index, string name, string color, double position)
        {
            Id = id;
            Index = index;
            Name = name;
            Color = color;
            Position = position;
            PlayerArea = new PlayerArea(index);
        }

        public List<Vector> GetPaddleEndCoords()
        {
            var vStart = PlayerArea.StartCoords; // vector coordinates of start end of paddle
            var vEnd = PlayerArea.EndCoords; // vector coordinates of end end of paddle

            double proportionalPaddleLength = 0.3; // will get from gamestate!!!!
            var adjustedPosition = Position * (1-proportionalPaddleLength) 
                + proportionalPaddleLength/2;

            // var end1 = new Vector(x1, y1);
            var end1 = vStart.Add(
                vEnd.Subtract(vStart).ScalarMultiply(
                    (adjustedPosition - proportionalPaddleLength / 2) 
                )
            );

            var end2 = vStart.Add(
                vEnd.Subtract(vStart).ScalarMultiply(
                    (adjustedPosition + proportionalPaddleLength / 2) 
                )
            );

            return new List<Vector>{ end1, end2 };
        }
    }
}
