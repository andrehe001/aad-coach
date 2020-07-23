using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureGameDay.Web.Models
{
    public class GameInfoForBackend
    {
        public Guid matchId { get; set; }
        public string challengerId { get; set; }
    }
}
