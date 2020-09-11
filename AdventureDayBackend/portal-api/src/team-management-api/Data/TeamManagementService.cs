using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        public bool AddTeam(Team newTeam)
        {
            _context.Add(newTeam);
            return this.SaveChanges();
        }

        public bool RenameTeam(int teamId, string newName)
        {
            var team = this.GetTeamById(teamId);
            team.Name = newName;
            _context.Update(team);
            return this.SaveChanges();
        }

        public bool UpdateTeam(Team team)
        {
            _context.Attach(team);
            _context.Update(team);
            return this.SaveChanges();
        }

        public bool CheckTeamNameFree(int teamId, string teamName)
        {
            return !_context.Teams.Any(t => t.Name.Equals(teamName));
        }

        public bool DeleteTeam(int id)
        {
            var team = this.GetTeamById(id);
            _context.Remove(team);
            return this.SaveChanges();
        }

        public IEnumerable<Team> GetAllTeams()
        {
            return _context.Teams.AsEnumerable();
        }

        public IEnumerable<Team> GetAllTeamsWithMembers()
        {
            return _context.Teams.Include(t => t.Members).AsEnumerable();
        }

        public Team GetTeamById(int teamId)
        {
            if (teamId == AppSettings.AdminTeamId)
            {
                return AppSettings.GetAdminTeam(_appSettings);
            }

            return _context.Teams.Where(team => team.Id == teamId).FirstOrDefault();
        }

        public Team GetTeamByIdWithMembers(int teamId)
        {
            if (teamId == AppSettings.AdminTeamId)
            {
                return AppSettings.GetAdminTeam(_appSettings);
            }

            return _context.Teams.Include(t => t.Members).Where(team => team.Id == teamId).FirstOrDefault();
        }

        public Team GetTeamByName(string name)
        {
            return _context.Teams.Where(team => team.Name == name).FirstOrDefault();
        }

        public bool RenameMember(int teamId, int memberId, string newDisplayName)
        {
            var member = this.GetMember(teamId, memberId);
            if (member != null)
            {
                member.DisplayName = newDisplayName;
                _context.Update(member);
                return this.SaveChanges();
            }
            else
            {
                return false;
            }
        }

        public bool AddMemberToTeam(int teamId, Member member)
        {
            var team = this.GetTeamByIdWithMembers(teamId);
            if (team != null)
            {
                if (team.Members == null)
                {
                    team.Members = new List<Member>();
                }
                team.Members.Add(member);
                _context.Update(team);
                return this.SaveChanges();
            }
            else
            {
                return false;
            }
        }

        public bool RemoveMemberFromTeam(int teamId, int memberId)
        {
            var team = this.GetTeamByIdWithMembers(teamId);
            if (team != null)
            {
                var member = team.Members.FirstOrDefault(m => m.Id == memberId);
                if (member != null)
                {
                    team.Members.Remove(member);
                    _context.Update(team);
                    return this.SaveChanges();
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<Member> GetMembers(int teamId)
        {
            return _context.Teams.Include(t => t.Members).FirstOrDefault(t => t.Id == teamId).Members.ToArray();
        }

        public Member GetMember(int teamId, int memberId)
        {
            return _context.Teams.FirstOrDefault(t => t.Id == teamId).Members.FirstOrDefault(m => m.Id == memberId);
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            if (model == null)
                return null;

            var hashedInput = AppSettings.HashString(_appSettings, model.Password);

            var team = _context.Teams.SingleOrDefault(x => x.Name == model.Teamname && x.TeamPassword == hashedInput);

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

            if (model.Teamname.Equals("admin") && model.Password.Equals(_appSettings.AdminPassword))
            {
                var token = generateJwtToken(AppSettings.GetAdminTeam(_appSettings));
                var response = new AuthenticateResponse(AppSettings.GetAdminTeam(_appSettings), token);
                response.IsAdmin = true;
                return response;
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
                Subject = new ClaimsIdentity(new[] { 
                    new Claim("teamId", team.Id.ToString()),
                    new Claim("isAdmin", team.Name.Equals(AppSettings.AdminTeamName).ToString()),
                    new Claim("teamName",team.Name),
                    new Claim("subscriptionid",team.SubscriptionId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _appSettings.JwtIssuer,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool SaveChanges()
        {
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
