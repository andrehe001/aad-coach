﻿using System;
using System.Collections.Generic;
using System.Text;

namespace team_management_data
{
    public interface ITeamDataService: IAuthenticationService
    {
        bool AddTeam(Team newTeam);
        bool UpdateTeam(Team team);
        bool CheckTeamNameFree(int teamId, string teamName);
        bool DeleteTeam(int teamId);

        bool RenameTeam(int teamId, string newName);

        bool RenameMember(int teamId, int memberId, string newDisplayName);
        bool AddMemberToTeam(int teamId, Member member);
        IEnumerable<Member> GetMembers(int teamId);
        Member GetMember(int teamId, int memberId);
        bool RemoveMemberFromTeam(int teamId, int memberId);
        IEnumerable<Team> GetAllTeams();
        IEnumerable<Team> GetAllTeamsWithMembers();

        Team GetTeamById(int teamId);
        Team GetTeamByIdWithMembers(int teamId);
        Team GetTeamByName(string teamName);
    }
}
