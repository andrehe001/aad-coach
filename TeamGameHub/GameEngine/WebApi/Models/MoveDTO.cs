using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamGameHub.GameEngine.WebApi.Models
{
    /// <summary>
    /// This is what the bot returns
    /// </summary>
    public class MoveDTO
    {
        public Move Move { get; set; }
        
        public decimal? Bet { get; set; }
    }
}
