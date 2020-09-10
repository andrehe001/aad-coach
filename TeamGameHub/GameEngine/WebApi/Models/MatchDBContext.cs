using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TeamGameHub.GameEngine.WebApi.Models
{
    public class MatchDBContext : DbContext
    {

        public MatchDBContext(DbContextOptions<MatchDBContext> options)
            : base(options)
        {
        }

        public DbSet<MatchResult> MatchResults{ get; set; }
        public DbSet<Turn> Turns{ get; set; }
    }
    
}


