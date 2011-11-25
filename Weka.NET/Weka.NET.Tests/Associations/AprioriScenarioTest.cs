using NUnit.Framework;
using Weka.NET.Tests.Core;
using Weka.NET.Associations;
using Weka.NET.Utils;

namespace Weka.NET.Tests.Associations
{
    [TestFixture]
    public class AprioriScenarioTest
    {
        [Test]
        [Ignore]
        public void ExtractingRulesFromWeatherNominal()
        {
            var apriori = new Apriori2(.2);

            var rules = apriori.BuildAssociationRules(TestSets.WeatherNominal());
        }
    }
}
