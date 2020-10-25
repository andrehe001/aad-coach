using AdventureDay.Runner.Model;
using AdventureDay.Runner.Players;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventureDay.Runner.Tests
{
    [TestClass]
    public class MatchReportTests
    {

        [TestMethod]
        public void TestMethod1()
        {
            var matchReport = MatchReport.FromMatchResponse(new MatchResponse()
                {Bet = 0.4m, MatchOutcome = Outcome.HumanBotWins});
            
            
            Assert.AreEqual(2, matchReport.Income);
        }
        
        [TestMethod]
        public void TestMethod2()
        {
            var matchReport = MatchReport.FromMatchResponse(new MatchResponse()
                {Bet = 0.5m, MatchOutcome = Outcome.HumanBotWins});
            
            
            Assert.AreEqual(3, matchReport.Income);
        }
    }
}