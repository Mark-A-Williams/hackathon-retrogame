using System;
using System.Linq;

namespace Web.Services
{
    public class GameCodeService : IGameCodeService
    {
        private const int Length = 4;
        private readonly Random _random = new Random();
        private readonly string _chars = "abcdefghijklmnopqrstuvwxyz0123456789";

        public string GetCode()
            => new string(
                Enumerable.Range(0, Length)
                .Select(o => GetChar())
                .ToArray());

        private char GetChar()
            => _chars[_random.Next(_chars.Length)];
    }
}
