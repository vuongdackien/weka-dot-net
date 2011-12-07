namespace Weka.NET.Tests.Associations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using Weka.NET.Associations;
    
    [TestFixture]
    public class AssociationRuleTest
    {
        [Test]
        public void AssociationRuleProperlyImplementsGetHashCodeAndEquals()
        {
            var rule = SomeRule();

            var rules = new HashSet<AssociationRule>();

            rules.Add(rule);

            rules.Add(rule);

            Assert.AreEqual(1, rules.Count);
        }

        [Test]
        public void CanCalculateConfidence()
        {
            var rule = SomeRule();

            Assert.AreEqual(rule.Confidence, rule.FullRule.Support / (double) rule.Premisse.Support);
        }

        [Test]
        public void AssociationRuleContainsSupport()
        {
            var rule = SomeRule();

            Assert.AreEqual(rule.FullRule.Support, rule.Support);
        }

        private static AssociationRule SomeRule()
        {
            var premisse = ItemSetTestBuilder.NewItemSet().WithItems(1d, null).WithAbsoluteSupport(4).Build();

            var consequence = ItemSetTestBuilder.NewItemSet().WithItems(null, 1d).WithAbsoluteSupport(3).Build();

            var premisseAndConsequence = ItemSetTestBuilder.NewItemSet().WithItems(1d, 1d).WithAbsoluteSupport(2).Build();

            var rule = new AssociationRule(premisse: premisse, consequence: consequence, fullRule: premisseAndConsequence);

            Console.WriteLine("SomeRule: " + rule.ToString());

            return rule;
        }
    }
}
