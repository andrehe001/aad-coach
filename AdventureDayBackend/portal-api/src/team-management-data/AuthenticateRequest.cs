using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace team_management_data
{
    public class AuthenticateRequest
    {
        [Required]
        public string Teamname { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
