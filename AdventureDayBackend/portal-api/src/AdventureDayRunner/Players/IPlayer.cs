using System.Threading;
using System.Threading.Tasks;

namespace AdventureDayRunner.Players
{
    public interface IPlayer
    {
        /// <summary>
        /// Returns the final match response. All internal play steps are handled by
        /// the player's "AI" ;-)
        /// </summary>
        /// <param name="cancellationToken">Cancel.</param>
        /// <returns>The final match response or null upon failure.</returns>
        Task<MatchReport> Play(CancellationToken cancellationToken);
        
        /// <summary>
        /// Name of the Player.
        /// </summary>
        string Name { get; }
    }
}