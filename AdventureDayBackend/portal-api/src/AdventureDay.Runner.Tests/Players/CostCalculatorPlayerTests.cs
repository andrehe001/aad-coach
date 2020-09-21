using AdventureDay.Runner.Players.PseudoPlayers;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AdventureDay.DataModel;
using AdventureDay.ManagementApi.Data;

namespace AdventureDay.Runner.Tests
{
    [TestClass]
    public class CostCalculatorPlayerTests
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            // Arrange
            var team = new Team { 
                SubscriptionId = Guid.Parse("73119864-58f7-4cb3-b1e5-98300bcc2557"),
                TenantId = Guid.Parse("72f988bf-86f1-41af-91ab-2d7cd011db47"),
            };

            var configValues = new Dictionary<string, string>
            {
               {"AzureSPClientId", "b9415f6c-a502-4cae-bec2-4691971f2"},
               {"AzureSPClientSecret", "ORL55.0ERKzbb0Fr6~_PG8CK8gAVk9"},
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues);
            var config = builder.Build();

            var player = new CostCalculatorPlayer(config, team, TimeSpan.FromSeconds(1));

            // Act
            var matchReport = await player.Play(CancellationToken.None);

            // Assert
            Assert.IsNotNull(matchReport);
        }
    }
}
