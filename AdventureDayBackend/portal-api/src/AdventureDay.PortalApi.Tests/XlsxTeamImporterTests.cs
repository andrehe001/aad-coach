using System;
using Xunit;
using System.Linq;
using AdventureDay.DataModel;
using System.Collections.Generic;

namespace AdventureDay.PortalApi.Tests
{
    public class XlsxTeamImporterTests
    {

        [Fact]
        public void ExtractTeamsIgnoresMissingPasswordColumn()
        {
            using (var importer = new Helpers.XlsxTeamImporter())
            {
                importer.LoadFromPath("AzureAdventureDay_AzurePreparation_NoTeamsPasswords.xlsx");
                var teams = importer.ExtractTeams();
                Assert.Equal(13, teams.Count());
                foreach (var team in teams)
                {
                    Assert.Empty(team.TeamPassword);
                    Assert.NotEmpty(team.Name);
                }
            }
        }

        [Fact]
        public void ExtractTeamsGets13TeamsFromBasicSample()
        {
            using (var importer = new Helpers.XlsxTeamImporter())
            {
                importer.LoadFromPath("AzureAdventureDay_AzurePreparation_BasicExample.xlsx");
                var teams = importer.ExtractTeams();
                Assert.Equal(13, teams.Count()); // Leaving admin team in there for now, check needs to happen higher level
            }
        }

        [Fact]
        public void ExtractTeamsGetsFirstTeamCorrectFromBasicSample()
        {
            using (var importer = new Helpers.XlsxTeamImporter())
            {
                importer.LoadFromPath("AzureAdventureDay_AzurePreparation_BasicExample.xlsx");
                var teams = importer.ExtractTeams();
                Assert.Equal("Team1", teams.ElementAt(0).Name);
                Assert.Equal("1ecb7d69-90b4-4f74-83b6-32a4500c2a2a", teams.ElementAt(0).SubscriptionId.ToString().ToLower());
                Assert.Equal("d8f1d6fc-21fa-4b94-bfe4-7813758630b5", teams.ElementAt(0).TenantId.ToString().ToLower());
                Assert.Equal("nASAtiOn", teams.ElementAt(0).TeamPassword);
            }
        }

        [Fact]
        public void ExtractMembersGetsAllMembersForOneTeamFromBasicSample()
        {
            using (var importer = new Helpers.XlsxTeamImporter())
            {
                var teams = CreateOneTeam();
                importer.LoadFromPath("AzureAdventureDay_AzurePreparation_BasicExample.xlsx");
                teams = importer.ExtractMembers(teams);
                Assert.Equal(6, teams.ElementAt(0).Members.Count());
            }
        }

        [Fact]
        public void ExtractMembersGetsCorrectMemberValuesForFirstMemberOfOneTeamFromBasicSample()
        {
            using (var importer = new Helpers.XlsxTeamImporter())
            {
                var teams = CreateOneTeam();
                importer.LoadFromPath("AzureAdventureDay_AzurePreparation_BasicExample.xlsx");
                teams = importer.ExtractMembers(teams);
                Assert.Equal("Team1A1@asmw211601.onmicrosoft.com", teams.ElementAt(0).Members.ElementAt(0).Username);
                Assert.Equal("Ktxf6289", teams.ElementAt(0).Members.ElementAt(0).Password);
            }
        }

        private static IEnumerable<Team> CreateOneTeam()
        {
            return new Team[] { new Team {
                Name = "Team1",
                SubscriptionId = new Guid("1ecb7d69-90b4-4f74-83b6-32a4500c2a2a"),
                TenantId = new Guid("d8f1d6fc-21fa-4b94-bfe4-7813758630b5"),
                TeamPassword = "nASAtiOn"
            } };
        }

        [Fact]
        public void ValidateFileFindsMissingSheet()
        {
            using (var importer = new Helpers.XlsxTeamImporter())
            {
                var teams = CreateOneTeam();
                importer.LoadFromPath("AzureAdventureDay_AzurePreparation_MissingSheet.xlsx");
                importer.ValidateFile();
                Assert.False(importer.FileValid);
                Assert.Single(importer.FileIssues);
                Assert.Equal("Sheet 'Azure Subscriptions' is missing.", importer.FileIssues.ElementAt(0));
            }
        }

        [Fact]
        public void ValidateFileFindsMissingColumn()
        {
            using (var importer = new Helpers.XlsxTeamImporter())
            {
                var teams = CreateOneTeam();
                importer.LoadFromPath("AzureAdventureDay_AzurePreparation_MissingColumn.xlsx");
                importer.ValidateFile();
                Assert.False(importer.FileValid);
                Assert.Single(importer.FileIssues);
                Assert.Equal("Column 'Username' in sheet 'User Accounts & SPs' is missing.", importer.FileIssues.ElementAt(0));
            }
        }

        [Fact]
        public void ValidateFileIsOkWithOurBasicExample()
        {
            using (var importer = new Helpers.XlsxTeamImporter())
            {
                var teams = CreateOneTeam();
                importer.LoadFromPath("AzureAdventureDay_AzurePreparation_BasicExample.xlsx");
                importer.ValidateFile();
                Assert.True(importer.FileValid);
                Assert.Empty(importer.FileIssues);
            }
        }

        [Fact]
        public void ValidateFileIsOkWithTeamsPasswordColumMissing()
        {
            using (var importer = new Helpers.XlsxTeamImporter())
            {
                var teams = CreateOneTeam();
                importer.LoadFromPath("AzureAdventureDay_AzurePreparation_NoTeamsPasswords.xlsx");
                importer.ValidateFile();
                Assert.True(importer.FileValid);
                Assert.Empty(importer.FileIssues);
            }
        }

    }
}
