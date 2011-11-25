using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Weka.NET.Associations;

namespace Weka.NET.Tests.Associations
{
    [TestFixture]
    public class AssociationRuleExtTest
    {
        [Test]
        public void EnsureConfidenceForRuleDividesConsequenceCounterByPremiseCounter()
        {
            var someConsequence = new ItemSet(new double?[]{1d});
            const int consequenceCount = 3;

            var somePremisse = new ItemSet(new double?[]{1d});
            const int premiseCount = 9;

            var rule = new AssociationRule(premise: somePremisse, premiseCount: premiseCount, consequence: someConsequence, consequenceCount: consequenceCount);

            var actual = rule.CalculateConfidence();

            Assert.AreEqual((double)premiseCount / (double)consequenceCount, actual, 0d);
        }
    }

    public class AssociationRuleTest
    {
    }
}
