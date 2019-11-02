using System;
using System.Threading.Tasks;

namespace Model
{
    internal class GameStateUpdater
    {
        private readonly GameState _original;

        public GameStateUpdater(GameState model)
        {
            _original = model;
        }

        public async Task<GameState> GetUpdatedGameState()
        {
            throw new NotImplementedException();
        }
    }
}
