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

        private List<Vector> GetPaddleEndCoords()
        {
            var areaStartCoords = PlayerArea.StartCoords; // v_s
            var areaEndCoords = PlayerArea.EndCoords; // v_e

            var proportionalPaddleLength = 0.3; // will get from gamestate!!!!
            var adjustedPosition = Position * (1-proportionalPaddleLength) + proportionalPaddleLength/2;
            
            var x1 = areaStartCoords.X + 
                (adjustedPosition - proportionalPaddleLength / 2) *
                (areaEndCoords.X - areaStartCoords.X);
                
            var y1 = areaStartCoords.Y + 
                (adjustedPosition - proportionalPaddleLength / 2) *
                (areaEndCoords.Y - areaStartCoords.Y);

            var x2 = areaStartCoords.X + 
                (adjustedPosition + proportionalPaddleLength / 2) *
                (areaEndCoords.X - areaStartCoords.X);

            var y2 = areaStartCoords.Y + 
                (adjustedPosition + proportionalPaddleLength / 2) *
                (areaEndCoords.Y - areaStartCoords.Y);

            var end1 = new Vector(x1, y1);
            var end2 = new Vector(x2, y2);

            return new List<Vector>{ end1, end2 };
        }
    }
}
