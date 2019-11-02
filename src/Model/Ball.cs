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

        // velocity of 1 means 1 unit per second, and the radius of the play region is 1 unit.
        public Vector Velocity { get; }
    }
}
