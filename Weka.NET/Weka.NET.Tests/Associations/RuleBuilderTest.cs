using NUnit.Framework;
using Weka.NET.Associations;
using System.Collections.Generic;
using Moq;
using Weka.NET.Core;

namespace Weka.NET.Tests.Associations
{
    [TestFixture]
    public class RuleBuilderTest
    {
        [Test]
        public void BuildingRulesWithTheExpectedConfidenceAndLiftAndSupport()
        {
            //Given
            var supportByItems = new Dictionary<ItemSet, int>();

            supportByItems[new ItemSet(new double?[] { 0, null, null, null })] = 4;

            supportByItems[new ItemSet(new double?[] { null, 0, null, null })] = 3;
            
            supportByItems[new ItemSet(new double?[] { null, null, 0, null })] = 3;

            supportByItems[new ItemSet(new double?[] { 0, 0, null, null })] = 3;

            supportByItems[new ItemSet(new double?[] { 0, null, 0, null })] = 2;

            //Given
            var dataSet = new Mock<IDataSet>();

            dataSet.SetupGet(d => d.Count).Returns(5);

            //When
            var builder = new RuleBuilder(.5);

            var rules = builder.BuildRules(dataSet.Object, supportByItems);

            foreach (var r in rules)
            {
                System.Console.WriteLine(r.ToString());
            }

            //Then

            Assert.AreEqual(4, rules.Count);

            Assert.AreEqual(new AssociationRule(premisse:new ItemSet(new double?[] { 0, null, null, null }), consequence:new ItemSet(new double?[] { null, 0, null, null }), confidence: 0.75, support: 0.6, lift: 1.25), rules[0]);

            Assert.AreEqual(new AssociationRule(premisse: new ItemSet(new double?[] { 0, null, null, null }), consequence: new ItemSet(new double?[] { null, null, 0, null }), confidence: 0.5, support: 0.4, lift: 1.25), rules[1]);

            Assert.AreEqual(new AssociationRule(premisse: new ItemSet(new double?[] { null, 0, null, null }), consequence: new ItemSet(new double?[] { 0, null, null, null }), confidence: 1d, support: 0.6, lift: 1.25), rules[2]);

            Assert.AreEqual(new AssociationRule(premisse: new ItemSet(new double?[] { null, null, 0, null }), consequence: new ItemSet(new double?[] { 0, null, null, null }), confidence: 2d/3d, support: 0.4, lift: 1.25), rules[3]);
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

            //Given
            var dataSet = new Mock<IDataSet>();

            dataSet.SetupGet(d => d.Count).Returns(6);

            //When
            var builder = new RuleBuilder(.75);

            var rule = builder.BuildRule(left, right, dataSet.Object, supportByItems);

            System.Console.WriteLine(rule);

            //Then
            Assert.AreEqual(new AssociationRule(premisse: left, consequence: right, confidence: .75, support: 0.5, lift: 1.5), rule);
        }

        [Test]
        public void CanCalculateConfidence()
        {
            //Given
            var ruleBuilder = new RuleBuilder(AnyConfidence());

            var supportByItems = new Dictionary<ItemSet, int>();

            var fullRuleItemSet = new ItemSet(1d, 2d);
            supportByItems[fullRuleItemSet] = 9;

            var premisseItemSet = new ItemSet(1d, null);
            supportByItems[premisseItemSet] = 4;

            //When
            var actualConfidence = ruleBuilder.CalculateConfidence(fullRuleItemSet, premisseItemSet, supportByItems);
        
            //Then
            Assert.AreEqual((double) supportByItems[fullRuleItemSet] / (double) supportByItems[premisseItemSet], actualConfidence);
        }

        private double AnyConfidence()
        {
            return 0;
        }
    }
}
