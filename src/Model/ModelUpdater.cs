using System;

namespace Model
{
    internal class ModelUpdater
    {
        private readonly GameState _original;

        public ModelUpdater(GameState model)
        {
            _original = model;
        }

        public GameState GetUpdatedModel()
        {
            throw new NotImplementedException();
        }
    }
}
