namespace Model
{
    public class Vector
    {
        public float X { get; }
        public float Y { get; }

        public Vector(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector Zero => new Vector(0, 0);
    }
}
