using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace team_management_data
{
    public class Member
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string DisplayName { get; set; }
    }
}
