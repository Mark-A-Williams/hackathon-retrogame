namespace Model
{
    public class Ball
    {
        public Ball(double x, double y, Vector velocity)
        {
            XPosition = x;
            YPosition = y;
            Velocity = velocity;
        }

        public double XPosition { get; }
        public double YPosition { get; }
        public Vector Velocity { get; }
    }
}
