using System.Collections.Concurrent;

namespace Model
{
    internal static class GameStateExtensions
    {
        public static Player WithPosition(this Player player, double position)
            => new Player(
                player.Id,
                player.Index,
                player.Color,
                position
            );
    }
}
