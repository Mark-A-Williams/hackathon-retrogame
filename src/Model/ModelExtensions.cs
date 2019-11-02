using System.Collections.Concurrent;

namespace Model
{
    internal static class ModelExtensions
    {
        public static Vector Add(this Vector vector1, Vector vector2)
            => new Vector(
                vector1.X + vector2.X,
                vector1.Y + vector2.Y
            );

        public static Vector Subtract(this Vector vector1, Vector vector2)
            => new Vector(
                vector1.X - vector2.X,
                vector1.Y - vector2.Y
            );

        public static Vector ScalarMultiply(this Vector vector, double scalar)
            => new Vector(
                vector.X * scalar,
                vector.Y * scalar
            );
    }
}
