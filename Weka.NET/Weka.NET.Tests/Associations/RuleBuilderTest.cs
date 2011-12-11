using NUnit.Framework;
using Weka.NET.Associations;
using System.Collections.Generic;

namespace Weka.NET.Tests.Associations
{
    [TestFixture]
    public class RuleBuilderTest
    {
        [Test]
        public void BuildingRulesFromItemSets()
        {
            //Given
            var supportByItems = new Dictionary<ItemSet, int>();

            supportByItems[new ItemSet(new double?[] { 0, null, null, null })] = 4;

            supportByItems[new ItemSet(new double?[] { null, 0, null, null })] = 3;
            
            supportByItems[new ItemSet(new double?[] { null, null, 0, null })] = 3;

            supportByItems[new ItemSet(new double?[] { 0, 0, null, null })] = 3;

            supportByItems[new ItemSet(new double?[] { 0, null, 0, null })] = 3;

            //When
            var builder = new RuleBuilder(.75);

            var rules = builder.BuildRules(supportByItems);

            //Then

            Assert.AreEqual(4, rules.Count);

            Assert.AreEqual(new AssociationRule(premisse:new ItemSet(new double?[] { 0, null, null, null }), consequence:new ItemSet(new double?[] { null, 0, null, null }), confidence: 0.75), rules[0]);

            Assert.AreEqual(new AssociationRule(premisse:new ItemSet(new double?[] { 0, null, null, null }), consequence:new ItemSet(new double?[] { null, null, 0, null }), confidence: 0.75), rules[1]);

            Assert.AreEqual(new AssociationRule(premisse: new ItemSet(new double?[] { null, 0, null, null }), consequence: new ItemSet(new double?[] { 0, null, null, null }), confidence: 1d), rules[2]);

            Assert.AreEqual(new AssociationRule(premisse: new ItemSet(new double?[] { null, null, 0, null }), consequence: new ItemSet(new double?[] { 0, null, null, null }), confidence: 1d), rules[3]);
        }

        [Test]
        public void BuildingOneAssociationRuleByPassingPremisseAndConsequence()
        {
            //Given
            var left = new ItemSet(null, 2d);

            var right = new ItemSet(1d, null);

            var fullRule = new ItemSet(1d, 2d);

            var supportByItems = new Dictionary<ItemSet, int>();

            supportByItems[left] = 4;

            supportByItems[right] = 4;

            supportByItems[fullRule] = 3;

            //When
            var builder = new RuleBuilder(.75);

            var rule = builder.BuildRule(left, right, supportByItems);

            //Then
            Assert.AreEqual(new AssociationRule(premisse: left, consequence: right, confidence: .75), rule);
        }

        [Test]
        public void CanCalculateConfidence()
        {
            int premisseSupport = 4;

            int fullRuleSupport = 5;

            Assert.AreEqual(fullRuleSupport / (double)premisseSupport
                , RuleBuilder.CalculateConfidence(fullRuleSupport, premisseSupport));
        }
    }
}
