﻿using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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
        public static readonly string AdminTeamName = "admin";
        public static readonly int AdminTeamId = 1337;

        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public string AdminPassword { get; set; }
        public string SqlConnectionString { get; set; }

        public string HashSalt { get; set; }

        public static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration["Authentication:SqlConnectionString"];
            return connectionString;
        }

        //public static string HashString(AppSettings settings, string input)
        //{
        //    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //        password: input,
        //        salt: settings.HashSalt,
        //        prf: KeyDerivationPrf.HMACSHA1,
        //        iterationCount: 10000,
        //        numBytesRequested: 256 / 8));
        //}

        private static Team adminTeam = null; 
        public static Team GetAdminTeam(AppSettings settings)
        {
            if (adminTeam == null)
            {
                adminTeam = new Team()
                {
                    Id = AppSettings.AdminTeamId,
                    Name = AppSettings.AdminTeamName,
                    TeamPassword = settings.AdminPassword
                };
            }

            return adminTeam;
        }
    }
}
