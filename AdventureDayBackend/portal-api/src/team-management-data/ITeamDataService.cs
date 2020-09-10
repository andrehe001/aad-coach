using System;
using System.Collections.Generic;
using System.Text;

namespace team_management_data
{
    public interface ITeamDataService: IAuthenticationService
    {
        void AddTeam(Team newTeam);

        void DeleteTeam(int id);

        IEnumerable<Team> GetAllTeams();

        Team GetTeamById(int id);
        Team GetTeamByName(string name);
    }
}
