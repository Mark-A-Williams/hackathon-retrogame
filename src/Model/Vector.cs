namespace Model
{
    public class Vector
    {
        public double X { get; }
        public double Y { get; }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector Zero => new Vector(0, 0);
    }
}
