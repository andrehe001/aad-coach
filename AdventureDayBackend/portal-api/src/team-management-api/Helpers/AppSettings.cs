using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using team_management_data;

namespace team_management_api.Helpers
{
    public class AppSettings
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public string AdminPassword { get; set; }
        public string SqlConnectionString { get; set; }

        public static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration["Authentication:SqlConnectionString"];
            return connectionString;
        }

        public static Team GetAdminTeam(AppSettings settings)
        {
            var adminTeam = new Team()
            {
                Id = 1337,
                Name = "Admins",
                TeamPassword = settings.AdminPassword
            };

            return adminTeam;
        }
    }
}
