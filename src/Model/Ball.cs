namespace Model
{
    public class Ball
    {
        public Ball(float x, float y, Vector velocity)
        {
            XPosition = x;
            YPosition = y;
            Velocity = velocity;
        }

        public float XPosition { get; }
        public float YPosition { get; }
        public Vector Velocity { get; }
    }
}
