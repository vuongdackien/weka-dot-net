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
        public void CanBuildSingletons()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithNominalAttribute
                    (name: "first_attribute", values: new[] { "first0", "first1" })
                .WithNominalAttribute
                    (name: "second_attribute", values: new[] { "second0", "second1" })

                .Build();

            var apriori = new Apriori(minSupport: 1);

            //When
            var singletons = apriori.BuildAssociationRules(dataSet);

            //Then
            Assert.AreEqual(4, singletons.Count());
            Assert.IsTrue(singletons.Contains(new ItemSet(counter: 0, items: new int?[] { null, 0 })));
            Assert.IsTrue(singletons.Contains(new ItemSet(counter: 0, items: new int?[] { null, 1 })));
            Assert.IsTrue(singletons.Contains(new ItemSet(counter: 0, items: new int?[] { 1, null })));
            Assert.IsTrue(singletons.Contains(new ItemSet(counter: 0, items: new int?[] { 1, null })));
        }

        [Test]
        public void CanFindLargeItemSet()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithNominalAttribute
                    (name: "first_attribute", values: new[] { "first0", "first1" })
                .WithNominalAttribute
                    (name: "second_attribute", values: new[] { "second0", "second1" })

                .AddData(values:new[] { "first0", "second0" })
                .AddData(values:new[] { "first0", "second1" })
                .AddData(values:new[] { "first0", "second1" })
                .AddData(values:new[] { "first0", "second1" })
                .AddData(values:new[] { "first1", "second1" })
                .AddData(values:new[] { "first1", "second1" })

                .Build();

            var apriori = new Apriori(minSupport: 1);
        }

        public void CanBuildExtractRulesForNominalDataSet()
        {
            var apriori = new Apriori(minSupport:10);

            var dataSet = TestSets.WeatherNominal();

            var rules = apriori.BuildAssociationRules(dataSet);

            //Ensure rules are generated as expected
        }

    
    }
}
