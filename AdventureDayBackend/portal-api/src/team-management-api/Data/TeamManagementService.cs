using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using team_management_api.Helpers;
using team_management_api.Models;
using team_management_data;

namespace team_management_api.Data
{
    public class TeamManagementService : ITeamDataService
    {
        private readonly TeamManagementContext _context;
        private readonly AppSettings _appSettings;

        public TeamManagementService(TeamManagementContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public void AddTeam(Team newTeam)
        {
            _context.Add(newTeam);
            _context.SaveChanges();
        }

        public void DeleteTeam(int id)
        {
            var team = this.GetTeamById(id);
            _context.Remove(team);
            _context.SaveChanges();
        }

        public IEnumerable<Team> GetAllTeams()
        {
            return _context.Teams.AsEnumerable();
        }

        public Team GetTeamById(int id)
        {
            return _context.Teams.Where(team => team.Id == id).FirstOrDefault();
        }

        public Team GetTeamByName(string name)
        {
            return _context.Teams.Where(team => team.Name == name).FirstOrDefault();
        }


        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            if (model == null)
                return null;

            var team = _context.Teams.SingleOrDefault(x => x.Name == model.Teamname && x.TeamPassword == model.Password);

            // return null if user not found
            if (team == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(team);

            return new AuthenticateResponse(team, token);
        }

        public AuthenticateResponse AuthenticateAdmin(AuthenticateRequest model)
        {
            if (model == null)
                return null;

            if (model.Teamname.Equals("Admin") && model.Password.Equals(_appSettings.AdminPassword))
            {
                var token = generateJwtToken(AppSettings.GetAdminTeam(_appSettings));
                return new AuthenticateResponse(AppSettings.GetAdminTeam(_appSettings), token);
            }
            else
                return null;
        }

        private string generateJwtToken(Team team)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", team.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
