using NUnit.Framework;
using Weka.NET.Associations;
using Weka.NET.Utils;
using System.Collections.Generic;

namespace Weka.NET.Tests.Associations
{
    public class ItemSetTest
    {
        [Test]
        public void CanCompareDifferentItemSets()
        {
            var someItemSet = ItemSetTestBuilder.NewItemSet().WithItems(null, 5).WithAnySupport().Build();

            var anotherItemSet = ItemSetTestBuilder.NewItemSet().WithItems(5, null).WithAnySupport().Build();

            Assert.IsFalse(someItemSet.Equals(anotherItemSet));
        }

        [Test]
        public void CanTestForIntersectingSets()
        {
            var someItemSet = ItemSetTestBuilder.NewItemSet().WithItems(null, 5).WithAnySupport().Build();

            var anotherItemSet = ItemSetTestBuilder.NewItemSet().WithItems(5, null).WithAnySupport().Build();

            Assert.IsFalse(someItemSet.Intersects(anotherItemSet));
        }

        [Test]
        public void CanCompareEquivalentItemSets()
        {
            var sameItemSets = ItemSetTestBuilder.NewItemSet().WithItems(null, 5).WithSupport(0).BuildMany(2);

            Assert.IsTrue(sameItemSets[0].Equals(sameItemSets[1]));
        }

        [Test]
        public void ItemSetProperlyImplementsGetHashCodeAndEquals()
        {
            var someHashSet = new HashSet<ItemSet>();

            var sameItemSets = ItemSetTestBuilder.NewItemSet().WithItems(1, 2, 3).WithSupport(0).BuildMany(5);

            someHashSet.AddAll(sameItemSets);

            Assert.AreEqual(1, someHashSet.Count);
        }

    }
}
