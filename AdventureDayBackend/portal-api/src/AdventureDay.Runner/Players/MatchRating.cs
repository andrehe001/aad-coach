namespace AdventureDay.Runner.Players
{
    public enum MatchRating
    {
        /// <summary>
        /// Match was played successfully. Win or loose will be determined
        /// based on the match outcome. 
        /// </summary>
        PlayedMatchSuccessfully, 
        
        /// <summary>
        /// There was an error running the match. Could also
        /// be triggered by a Hack or a Cancellation. The reason
        /// field provides details.
        /// </summary>
        Error,
        
        /// <summary>
        /// Any result of this match should be ignored and not
        /// appear in the statistics. 
        /// </summary>
        Ignore
    }
}