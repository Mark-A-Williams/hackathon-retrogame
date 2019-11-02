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
    }
}
