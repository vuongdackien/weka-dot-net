using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Weka.NET.Associations;
using Weka.NET.Core;

namespace Weka.NET.Tests.Associations
{
    [TestFixture]
    public class ItemSetTest
    {
        [Test]
        public void TestingIfItemsContainsAnInstance()
        {
            var items = new ItemSet(1d, 2d);

            var instance = new Instance(new List<double?>{1d, 2d});

            Assert.IsTrue(items.ContainedBy(instance));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotMergeItemsWithDifferentSize()
        {
            var left = new ItemSet(new List<double?> { 1d, null, 1d });

            var right = new ItemSet(new List<double?> { null, 1d});

            left.Union(right);
        }

        [Test]
        public void IntersectingNonIntersectedItems()
        {
            var left = new ItemSet(1d, null);

            var right = new ItemSet(null, 2d);

            Assert.AreEqual(0, left.IntersectsCount(right));
        }

        [Test]
        public void IntersectingIntersectedItems()
        {
            var left = new ItemSet(1d, null);

            var right = new ItemSet(1d, 1d);

            Assert.AreEqual(1, left.IntersectsCount(right));
        }

        [Test]
        public void IntersectingItemsThatDifferByValue()
        {
            var left = new ItemSet(1d, null);

            var right = new ItemSet(2d, 1d);

            Assert.AreEqual(0, left.IntersectsCount(right));
        }


        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void MergingOnlyWorksIfOneValueIsNull()
        {
            var left = new ItemSet(1d, null);

            var right = new ItemSet(2d, null);

            left.Union(right);
        }

        [Test]
        public void CanMergeItemsWithTheSameSize()
        {
            var left = new ItemSet(new List<double?> { 1d, null, 1d });

            var right = new ItemSet(new List<double?> { null, 1d, null });

            Assert.AreEqual(new ItemSet(1d, 1d, 1d), left.Union(right));
        }

        [Test]
        public void CanMergeOneItemSetWithItself()
        {
            var itemSet = new ItemSet(new List<double?> { 1d, null, 1d });

            Assert.AreEqual(itemSet, itemSet.Union(itemSet));
        }

        [Test]
        public void CanPutItemsInAHashSet()
        {
            var set = new HashSet<ItemSet>();

            set.Add(new ItemSet(new List<double?> { 1d, 1d }));

            set.Add(new ItemSet(new List<double?> { 1d, 1d }));

            var removed = set.Remove(new ItemSet(new List<double?> { 1d, 1d }));

            Assert.AreEqual(0, set.Count);
        }
    }
}
