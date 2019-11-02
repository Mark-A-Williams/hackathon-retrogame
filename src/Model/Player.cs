using System;
using System.Collections.Generic;

namespace Model
{
    public class Player
    {
        public int Index { get; }
        public string Color { get; }
        public float Position { get; } // position of centre of paddle from 0 to 1
        public PlayerArea PlayerArea { get; set; }
        public List<Vector> PaddleEndCoords {get; set; }

        public Player(int index, string color, float position)
        {
            Index = index;
            Color = color;
            Position = position;
            PlayerArea = new PlayerArea(index);
        }

        
    }
}
