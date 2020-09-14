using System;

namespace AdventureDayRunner.Players
{
    public class MatchCancelledException : Exception
    {
        public MatchCancelledException()
        {
        }

        public MatchCancelledException(string message)
            : base(message)
        {
        }

        public MatchCancelledException(string message, Exception inner)
            : base(message, inner)
        {
        } 
    }
}