using System;

namespace Model
{
    public class AddPlayerResult
    {
        public Guid Id { get; }
        public string Color { get; }

        public AddPlayerResult(Guid id, string color)
        {
            Id = id;
            Color = color;
        }
    }
}
