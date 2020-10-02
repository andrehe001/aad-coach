using AdventureDay.DataModel.Runner;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventureDay.Runner.Tests
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var runnerProperties = RunnerProperties.CreateDefault();
            var phaseConfig = runnerProperties.PhaseConfigurations.ToJson();
            Assert.IsNotNull(phaseConfig);
        }
    }
}
