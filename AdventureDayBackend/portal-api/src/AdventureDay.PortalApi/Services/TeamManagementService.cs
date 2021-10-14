using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AdventureDay.DataModel;
using AdventureDay.PortalApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AdventureDay.PortalApi.Services
{
    public class TeamManagementService : ITeamDataService
    {
        private readonly AdventureDayBackendDbContext _context;
        private readonly AppSettings _appSettings;

        public TeamManagementService(AdventureDayBackendDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public bool AddTeam(Team newTeam)
        {
            _context.Teams.Add(newTeam);
            SaveChanges();

            _context.TeamScores.Add(new TeamScore()
            {
                TeamId = newTeam.Id,
                Costs = 0,
                Income = 0,
                Errors = 0,
                Loses = 0,
                Wins = 0
            });

            return SaveChanges();
        }

        public bool AddTeamsFromXslx(string xlsxFilePath)
        {
            using (var importer = new XlsxTeamImporter())
            {
                importer.LoadFromPath(xlsxFilePath);
                if (!importer.FileValid)
                {
                    // TODO: How to get the validation details to the client? Like which column might be missing or such...
                    return false;
                }

                var teams = importer.ExtractTeams();
                teams = importer.ExtractMembers(teams);
                foreach (var team in teams)
                {
                    if (team.Name.Equals(AppSettings.AdminTeamName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(team.TeamPassword))
                    {
                        team.TeamPassword = PasswordGenerator.GetPassword();
                    }

                    if (!this.CheckTeamNameFree(team.Name))
                    {
                        // This will overwrite existing teams whenever we do an xlsx import
                        // TODO: Check whether that works and makes sense
                        // currently leads to
                        // Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while updating the entries. See the inner exception for details.
                        //--->Microsoft.Data.SqlClient.SqlException(0x80131904): The INSERT statement conflicted with the FOREIGN KEY constraint "FK_TeamScores_Teams_TeamId".The conflict occurred in database "master", table "dbo.Teams", column 'Id'.

                        var existingTeam = this.GetTeamByName(team.Name);
                        this.DeleteTeam(existingTeam.Id);
                    }

                    this.AddTeam(team);
                }
            }

            return true;
        }

        

        public bool RenameTeam(int teamId, string newName)
        {
            Team team = GetTeamById(teamId);
            team.Name = newName;
            _context.Update(team);
            return SaveChanges();
        }

        public bool UpdateGameEngineUri(int teamId, string newUri)
        {
            Team team = GetTeamById(teamId);
            team.GameEngineUri = newUri;
            _context.Update(team);
            return SaveChanges();
        }

        public bool UpdateTeam(Team team)
        {
            _context.Attach(team);
            _context.Update(team);
            return SaveChanges();
        }

        public bool CheckTeamNameFree(string teamName)
        {
            return !_context.Teams.Any(t => t.Name.ToLower() == teamName.ToLower());
        }

        public bool DeleteTeam(int id)
        {
            Team team = GetTeamById(id);
            _context.Remove(team);
            return SaveChanges();
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
            return _context.Teams.Where(team => team.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public bool AddMemberToTeam(int teamId, Member member)
        {
            Team team = GetTeamByIdWithMembers(teamId);
            if (team != null)
            {
                if (team.Members == null)
                {
                    team.Members = new List<Member>();
                }
                team.Members.Add(member);
                _context.Update(team);
                return SaveChanges();
            }
            else
            {
                return false;
            }
        }

        public bool AddMemberToTeam(string teamName, Member member)
        {
            Team team = GetTeamByName(teamName);
            if (team != null)
            {
                if (team.Members == null)
                {
                    team.Members = new List<Member>();
                }
                team.Members.Add(member);
                _context.Update(team);
                return SaveChanges();
            }
            else
            {
                return false;
            }
        }

        public bool RemoveMemberFromTeam(int teamId, int memberId)
        {
            Team team = GetTeamByIdWithMembers(teamId);
            if (team != null)
            {
                Member member = team.Members.FirstOrDefault(m => m.Id == memberId);
                if (member != null)
                {
                    team.Members.Remove(member);
                    _context.Update(team);
                    return SaveChanges();
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
            {
                return null;
            }

            string hashedInput = AppSettings.HashString(_appSettings, model.Password);

            Team team = _context.Teams.SingleOrDefault(x => x.Name.ToLower() == model.Teamname.ToLower() && x.TeamPassword == hashedInput);

            // return null if user not found
            if (team == null)
            {
                return null;
            }

            // authentication successful so generate jwt token
            string token = generateJwtToken(team);

            return new AuthenticateResponse(team, token);
        }

        public AuthenticateResponse AuthenticateAdmin(AuthenticateRequest model)
        {
            if (model == null)
            {
                return null;
            }

            if (model.Teamname.Equals("admin", StringComparison.OrdinalIgnoreCase) && model.Password.Equals(_appSettings.AdminPassword))
            {
                string token = generateJwtToken(AppSettings.GetAdminTeam(_appSettings));
                AuthenticateResponse response = new AuthenticateResponse(AppSettings.GetAdminTeam(_appSettings), token)
                {
                    IsAdmin = true
                };
                return response;
            }
            else
            {
                return null;
            }
        }

        private string generateJwtToken(Team team)
        {
            // generate token that is valid for 7 days
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.JwtKey);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
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
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool SaveChanges()
        {
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
