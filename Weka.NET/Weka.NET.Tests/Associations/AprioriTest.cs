using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Associations;
using Weka.NET.Tests.Core;
using NUnit.Framework;
using Weka.NET.Core;

namespace Weka.NET.Tests.Associations
{
    [TestFixture]
    public class AprioriTest
    {
        [Test]
        public void CanBuildExtractRulesForNominalDataSet()
        {
            var apriori = new Apriori(minSupport:10);

            var dataSet = TestSets.WeatherNominal();

            var rules = apriori.BuildAssociationRules(dataSet);

            //Ensure rules are generated as expected
        }
    }

    [TestFixture]
    public class ItemSetTest
    {
        [Test]
        public void ContainedByReturnsTrueIfInstanceContainsItemSet()
        {
            var someInstance = new Instance(values: new List<double?> { 1d, 2d, 3d });

            var someItemSet = new ItemSet(counter: 1,  items: new List<int?>{ null, 2, 3 } );

            Assert.IsTrue( someItemSet.ContainedBy(someInstance) );
        }

        [Test]
        public void ContainedByReturnsFalseIfInstanceDoesntContainItemSet()
        {
            var someInstance = new Instance(values: new List<double?> { 1d, 2d });

            var someItemSet = new ItemSet(counter: 1, items: new List<int?> { null, 5 });

            Assert.IsFalse(someItemSet.ContainedBy(someInstance));
        }

        [Test]
        public void EnsureConfidenceForRuleDividesConsequenceCounterByPremiseCounter()
        {
            var consequence = new ItemSet(counter: 10, items: new List<int?>{1,2});

            var premise = new ItemSet(counter: 10, items: new List<int?> { 1, 2 });

            double actual = ItemSet.ConfidenceForRule(premise, consequence);

            Assert.AreEqual((double)consequence.Counter / (double)premise.Counter, actual, 0d);
        }

    }


}
