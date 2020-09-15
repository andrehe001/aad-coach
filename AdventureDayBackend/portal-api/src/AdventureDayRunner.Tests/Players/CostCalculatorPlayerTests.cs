using AdventureDayRunner.Players.PseudoPlayers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;
using team_management_api.Data;

namespace AdventureDayRunner.Tests
{
    [TestClass]
    public class CostCalculatorPlayerTests
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            // Arrange
            var team = new Team();
            // TODO: Load config
            var player = new CostCalculatorPlayer(null, team, TimeSpan.FromSeconds(1));

            // Act
            var matchReport = await player.Play(CancellationToken.None);

            // Assert
            Assert.IsNotNull(matchReport);
        }
    }
}
