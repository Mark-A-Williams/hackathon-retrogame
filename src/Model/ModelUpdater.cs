namespace Model
{
    internal class ModelUpdater
    {
        private readonly object _original;

        public ModelUpdater(object model)
        {
            _original = model;
        }

        public object GetUpdatedModel()
        {
            return new object();
        }
    }
}
