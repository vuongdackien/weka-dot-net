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
    public class ItemSetTest
    {
        [Test]
        public void ContainedBy()
        {
            var someInstance = new Instance { Weight = 1.0, Values = new List<double?> { 1d, 2d, 3d } };

            var someItemSet = new ItemSet { Items = new List<int?>{ null, 2, 3 } };

            Assert.IsTrue( someItemSet.ContainedBy(someInstance) );
             
        }

    }

    [TestFixture]
    public class AprioriTest
    {
        Apriori apriori;

        [SetUp]
        public void SetupApriori()
        {
            apriori = new Apriori();
        }

        [Test]
        public void CanBuildAssociationRules()
        {
            var dataSet = DataSetTestBuilder.AnyDataSet().Build();

            apriori.BuildAssociationRules(dataSet);
        }
    }
}
