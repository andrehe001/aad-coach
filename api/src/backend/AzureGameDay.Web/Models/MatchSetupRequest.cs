using System;
using System.ComponentModel.DataAnnotations;

namespace AzureGameDay.Web.Models
{
    
    public class MatchSetupRequest
    {
        public Guid ChallengerId { get; set; }
    }
}
