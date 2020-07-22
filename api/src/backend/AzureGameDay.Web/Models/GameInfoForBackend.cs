using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureGameDay.Web.Models
{
    public class GameInfoForBackend
    {
        public Guid MatchId { get; set; }
        public string Challenger { get; set; }
    }
}
