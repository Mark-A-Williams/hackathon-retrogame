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
        public PlayerArea PlayerArea { get; set; }
        public List<Vector> PaddleEndCoords {get; set; }
        public double Position { get; } // position of centre of paddle from 0 to 1

        public Player(Guid id, int index, string name, string color, double position)
        {
            Id = id;
            Index = index;
            Name = name;
            Color = color;
            Position = position;
            PlayerArea = new PlayerArea(index);
        }
    }
}
