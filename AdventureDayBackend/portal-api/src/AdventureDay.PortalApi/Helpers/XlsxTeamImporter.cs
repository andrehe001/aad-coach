using AdventureDay.DataModel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventureDay.PortalApi.Helpers
{
    public class XlsxTeamImporter : IDisposable
    {
        // TODO: This could be more robust by using Excel Ranges instead of sheet names or numbers

        private const string SHEET_TEAMS = "Azure Subscriptions";
        private const string SHEET_MEMBERS = "User Accounts & SPs";

        private const string COLUMN_TEAMS_TEAMNAME = "Teamname";
        private const string COLUMN_TEAMS_SUBSCRIPTION = "Subscription ID";
        private const string COLUMN_TEAMS_TENANT = "Tenant";
        private const string COLUMN_TEAMS_PASSWORD = "Password";

        private const string COLUMN_MEMBERS_TEAMNAME = "Teamname";
        private const string COLUMN_MEMBERS_USERNAME = "Username";
        private const string COLUMN_MEMBERS_PASSWORD = "Password";

        private SpreadsheetDocument _document = null;
        private FileStream _stream = null;

        private bool _FileValid = true;
        private List<String> _FileIssues = new List<string>();
        private bool disposedValue;

        public bool FileValid { get { return _FileValid; } }
        public IEnumerable<string> FileIssues { get { return _FileIssues; } }

        public IEnumerable<Team> ExtractTeams()
        {
            CheckDocumentIsLoaded();

            var sheetId = XlsxHelper.GetSheetId(SHEET_TEAMS, _document);
            var sheet = (_document.WorkbookPart.GetPartById(sheetId) as WorksheetPart).Worksheet;
            var table = _document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First().SharedStringTable;
            var columnIndices = new Dictionary<string, int> {
                { COLUMN_TEAMS_TEAMNAME, XlsxHelper.GetColumnIndex(COLUMN_TEAMS_TEAMNAME, sheet, table) } ,
                { COLUMN_TEAMS_SUBSCRIPTION, XlsxHelper.GetColumnIndex(COLUMN_TEAMS_SUBSCRIPTION, sheet, table) } ,
                { COLUMN_TEAMS_TENANT, XlsxHelper.GetColumnIndex(COLUMN_TEAMS_TENANT, sheet, table) } ,
                { COLUMN_TEAMS_PASSWORD, XlsxHelper.GetColumnIndex(COLUMN_TEAMS_PASSWORD, sheet, table) } ,
            };
            var rows = sheet.Descendants<Row>().Skip(1);
            var teams = new List<Team>();
            foreach (var row in rows)
            {
                var teamName = XlsxHelper.GetValue(row.Descendants<Cell>().ElementAt(columnIndices[COLUMN_TEAMS_TEAMNAME]), table);
                var subscriptionId = XlsxHelper.GetValue(row.Descendants<Cell>().ElementAt(columnIndices[COLUMN_TEAMS_SUBSCRIPTION]), table);
                var tenantId = XlsxHelper.GetValue(row.Descendants<Cell>().ElementAt(columnIndices[COLUMN_TEAMS_TENANT]), table);
                var password = XlsxHelper.GetValue(row.Descendants<Cell>().ElementAt(columnIndices[COLUMN_TEAMS_PASSWORD]), table);
                var team = new Team()
                {
                    Name = teamName,
                    SubscriptionId = new Guid(subscriptionId),
                    TenantId = new Guid(tenantId),
                    TeamPassword = password,
                };
                teams.Add(team);
            }

            return teams;
        }

        public IEnumerable<Team> ExtractMembers(IEnumerable<Team> teamsToExtractMembersFor)
        {
            CheckDocumentIsLoaded();

            var sheetId = XlsxHelper.GetSheetId(SHEET_MEMBERS, _document);
            var sheet = (_document.WorkbookPart.GetPartById(sheetId) as WorksheetPart).Worksheet;
            var table = _document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First().SharedStringTable;
            var columnIndices = new Dictionary<string, int> {
                { COLUMN_MEMBERS_TEAMNAME, XlsxHelper.GetColumnIndex(COLUMN_MEMBERS_TEAMNAME, sheet, table) } ,
                { COLUMN_MEMBERS_USERNAME, XlsxHelper.GetColumnIndex(COLUMN_MEMBERS_USERNAME, sheet, table) } ,
                { COLUMN_MEMBERS_PASSWORD, XlsxHelper.GetColumnIndex(COLUMN_MEMBERS_PASSWORD, sheet, table) } ,
            };
            var rows = sheet.Descendants<Row>().Skip(1);
            foreach (var team in teamsToExtractMembersFor)
            {
                var rowsForThisTeam = rows.Where(r => XlsxHelper.GetValue(r.Descendants<Cell>().ElementAt(columnIndices[COLUMN_MEMBERS_TEAMNAME]), table).Equals(team.Name, StringComparison.OrdinalIgnoreCase));
                foreach (var row in rowsForThisTeam)
                {
                    var username = XlsxHelper.GetValue(row.Descendants<Cell>().ElementAt(columnIndices[COLUMN_MEMBERS_USERNAME]), table);
                    var password = XlsxHelper.GetValue(row.Descendants<Cell>().ElementAt(columnIndices[COLUMN_MEMBERS_PASSWORD]), table);
                    var member = new DataModel.Member { Password = password, Username = username };
                    if (team.Members == null)
                    {
                        team.Members = new List<DataModel.Member>();
                    }

                    team.Members.Add(member);
                }
            }

            return teamsToExtractMembersFor;
        }

        public void LoadFromPath(string path)
        {
            if (_document != null)
            {
                throw new ApplicationException("There is already one doc loaded.");
            }

            _stream = File.OpenRead(path);
            _document = SpreadsheetDocument.Open(_stream, false);
            ValidateFile();
        }

        public void ValidateFile()
        {
            CheckDocumentIsLoaded();
            _FileValid = true;
            _FileIssues.Clear();

            var table = _document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First().SharedStringTable;
            string sheetId;
            sheetId = CheckSheet(SHEET_MEMBERS);
            if (!string.IsNullOrEmpty(sheetId))
            {
                var sheet = (_document.WorkbookPart.GetPartById(sheetId) as WorksheetPart).Worksheet;
                CheckColumn(COLUMN_MEMBERS_TEAMNAME, sheet, table, SHEET_MEMBERS);
                CheckColumn(COLUMN_MEMBERS_USERNAME, sheet, table, SHEET_MEMBERS);
                CheckColumn(COLUMN_MEMBERS_PASSWORD, sheet, table, SHEET_MEMBERS);
            }

            sheetId = CheckSheet(SHEET_TEAMS);
            if (!string.IsNullOrEmpty(sheetId))
            {
                var sheet = (_document.WorkbookPart.GetPartById(sheetId) as WorksheetPart).Worksheet;
                CheckColumn(COLUMN_TEAMS_TEAMNAME, sheet, table, SHEET_TEAMS);
                CheckColumn(COLUMN_TEAMS_SUBSCRIPTION, sheet, table, SHEET_TEAMS);
                CheckColumn(COLUMN_TEAMS_TENANT, sheet, table, SHEET_TEAMS);
                CheckColumn(COLUMN_TEAMS_PASSWORD, sheet, table, SHEET_TEAMS);
            }
        }

        private string CheckSheet(string name)
        {
            var sheetId = XlsxHelper.GetSheetId(name, _document);
            var isMissing = string.IsNullOrEmpty(sheetId);
            if (isMissing)
            {
                _FileValid = false;
                _FileIssues.Add($"Sheet '{name}' is missing.");
            }

            return sheetId;
        }

        private int CheckColumn(string name, Worksheet sheet, SharedStringTable table, string sheetName)
        {
            var index = XlsxHelper.GetColumnIndex(name, sheet, table);
            var isMissing = index < 0;
            if (isMissing)
            {
                _FileValid = false;
                _FileIssues.Add($"Column '{name}' in sheet '{sheetName}' is missing.");
            }

            return index;
        }

        private void CheckDocumentIsLoaded()
        {
            if (_document == null)
            {
                throw new ApplicationException("No document loaded yet.");
            }
        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_document != null) _document.Dispose();
                    if (_stream != null) _stream.Dispose();
                }

                disposedValue = true;
            }
        }

        ~XlsxTeamImporter()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
