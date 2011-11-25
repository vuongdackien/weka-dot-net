using System.Collections.Generic;
using Weka.NET.Associations;
using NUnit.Framework;
using Weka.NET.Core;

namespace Weka.NET.Tests.Associations
{
    [TestFixture]
    public class ItemSetTest
    {
        [Test]
        public void ContainedByReturnsTrueIfInstanceContainsItemSet()
        {
            var someInstance = new Instance(values: new List<double?> { 1d, 2d, 3d });

            var someItemSet = new ItemSet(items: new List<double?> { null, 2, 3 });

            Assert.IsTrue(someItemSet.ContainedBy(someInstance));
        }

        [Test]
        public void ContainedByReturnsFalseIfInstanceDoesntContainItemSet()
        {
            var someInstance = new Instance(values: new List<double?> { 1d, 2d });

            var someItemSet = new ItemSet(items: new List<double?> { null, 5 });

            Assert.IsFalse(someItemSet.ContainedBy(someInstance));
        }

        [Test]
        public void ItemSetImplementsEquatable()
        {
            var someItemSet = new ItemSet(items: new List<double?> { null, 5 });

            var anotherItemSet = new ItemSet(items: new List<double?> { null, 5 });

            Assert.IsTrue(someItemSet.Equals(anotherItemSet));
        }

        [Test]
        public void ComparingItemSetsWithEqualsOperator()
        {
            var actual = new HashSet<ItemSet>();

            var someItemSet = new ItemSet(items: new List<double?> { null, 5 });

            actual.Add(someItemSet);

            var anotherItemSet = new ItemSet(items: new List<double?> { null, 5 });

            actual.Add(anotherItemSet);

            var expected = new HashSet<ItemSet>();

            expected.Add(anotherItemSet);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EnsureConfidenceForRuleDividesConsequenceCounterByPremiseCounter()
        {
            const int consequenceCount = 3;

            const int premiseCount = 9;

            double actual = ItemSet.ConfidenceForRule(premiseCount, consequenceCount);

            Assert.AreEqual((double)premiseCount / (double)consequenceCount, actual, 0d);
        }


    }
}
