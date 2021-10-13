using AdventureDay.DataModel;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureDay.PortalApi.Helpers
{
    public class XlsxTeamImporter
    {
        public IEnumerable<Team> ExtractTeams(SpreadsheetDocument xlsx)
        {
            return new Team[] { };
        }
    }
}
