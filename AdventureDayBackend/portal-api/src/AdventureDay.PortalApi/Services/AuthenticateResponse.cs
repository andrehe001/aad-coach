using System;
using AdventureDay.DataModel;
using AdventureDay.PortalApi.Data;

namespace AdventureDay.PortalApi.Services
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Token { get; set; }

        public string Teamname { get; set; }

        public Guid SubcriptionId { get; set; }

        public bool IsAdmin { get; set; }

        public AuthenticateResponse(Team team, string token)
        {
            Id = team.Id;
            Teamname = team.Name;
            Token = token;
        }
    }
}
