namespace Model
{
    public class Ball
    {
        public Ball(Vector position, Vector velocity)
        {
            Position = position;
            Velocity = velocity;
        }

        public Vector Position { get; }
        public Vector Velocity { get; }
    }
}
