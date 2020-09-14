using System;

namespace AdventureDayRunner.Players
{
    public class MatchCanceledException : Exception
    {
        public MatchCanceledException()
        {
        }

        public MatchCanceledException(string message)
            : base(message)
        {
        }

        public MatchCanceledException(string message, Exception inner)
            : base(message, inner)
        {
        } 
    }
}