using System;

namespace TeamGameHub.GameEngine.WebApi.Utils
{
    public static class Rand
    {
        private static readonly Random Rnd = new Random();
        
        public static int GetRandom(int min = Int32.MinValue, int max = Int32.MaxValue)
        {
            return Rnd.Next(min, max);
        }
    }
}