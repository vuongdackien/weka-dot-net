﻿using System;
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
    public class ItemSetTest
    {
        [Test]
        public void ContainedByReturnsTrueIfInstanceContainsItemSet()
        {
            var someInstance = new Instance(values: new List<double?> { 1d, 2d, 3d });

            var someItemSet = new ItemSet(items: new List<int?> { null, 2, 3 });

            Assert.IsTrue(someItemSet.ContainedBy(someInstance));
        }

        [Test]
        public void ContainedByReturnsFalseIfInstanceDoesntContainItemSet()
        {
            var someInstance = new Instance(values: new List<double?> { 1d, 2d });

            var someItemSet = new ItemSet(items: new List<int?> { null, 5 });

            Assert.IsFalse(someItemSet.ContainedBy(someInstance));
        }

        [Test]
        public void EnsureConfidenceForRuleDividesConsequenceCounterByPremiseCounter()
        {
            var consequenceCount = 3;
            var premiseCount = 9;

            double actual = ItemSet.ConfidenceForRule(premiseCount, consequenceCount);

            Assert.AreEqual((double)premiseCount / (double)consequenceCount, actual, 0d);
        }
    }
}
