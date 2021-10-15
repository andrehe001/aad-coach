using System;
using Xunit;
using AdventureDay.PortalApi.Helpers;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace AdventureDay.PortalApi.Tests
{
    public class XlsxHelperTests
    {
        [Theory]
        [InlineData("Instructions", "rId1")]
        [InlineData("Azure Subscriptions", "rId2")]
        [InlineData("User Accounts & SPs", "rId3")]
        [InlineData("Non Existent", "")]
        public void GetSheetIndexGetsCorrectSheetsForExactNameMatches(string sheetName, string expectedId)
        {
            using (var stream = File.OpenRead("AzureAdventureDay_AzurePreparation_BasicExample.xlsx"))
            using (var doc = SpreadsheetDocument.Open(stream, false))
            {
                var actualId = XlsxHelper.GetSheetId(sheetName, doc);
                Assert.Equal(expectedId, actualId);
            }
        }

        [Theory]
        [InlineData("Teamname", 0)]
        [InlineData("UserNAME", 1)]
        [InlineData("Password", 2)]
        [InlineData("Non Existent", -1)]
        public void GetColumnIndexGetsCorrectColumnsForCaseInsensitivePartialNameMatches(string columnName, int expectedIndex)
        {
            using (var stream = File.OpenRead("AzureAdventureDay_AzurePreparation_BasicExample.xlsx"))
            using (var doc = SpreadsheetDocument.Open(stream, false))
            {
                var sheet = (doc.WorkbookPart.GetPartById("rId3") as WorksheetPart).Worksheet; // User Accounts & SPs
                var table = doc.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First().SharedStringTable;
                var actualIndex = XlsxHelper.GetColumnIndex(columnName, sheet, table);
                Assert.Equal(expectedIndex, actualIndex);
            }
        }
    }
}
