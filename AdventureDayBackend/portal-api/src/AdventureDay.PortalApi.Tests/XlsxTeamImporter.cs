using System;
using Xunit;
using AdventureDay.PortalApi.Helpers;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AdventureDay.PortalApi.Tests
{
    public class XlsxTeamImporter
    {
        [Fact]
        public void ImportXlsxGetsTeamStructure()
        {
            var importer = new Helpers.XlsxTeamImporter();
            var xlsx = SpreadsheetDocument.Open("AzureAdventureDay_AzurePreparation_BasicExample.xlsx", false);
            var teams = importer.ExtractTeams(xlsx);
            Assert.NotNull(teams);
        }
    }
}
