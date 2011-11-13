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
        [ExpectedException(typeof(ArgumentException))]
        public void CannotBuildSingletonsForNumericAttributes()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithNumericAttribute
                    (name: "some_numeric_attribute").Build();

            //When
            new Apriori(minSupport: 1).BuildSingletons(dataSet.Attributes);

            //Then expect exception
        }

        [Test]
        public void CanBuildSingletonsForNominalAttributes()
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
            var singletons = apriori.BuildSingletons(dataSet.Attributes);

            //Then
            Assert.AreEqual(4, singletons.Count());
            Assert.IsTrue(singletons.Contains(new ItemSet(counter: 0, items: new int?[] { null, 0 })));
            Assert.IsTrue(singletons.Contains(new ItemSet(counter: 0, items: new int?[] { null, 1 })));
            Assert.IsTrue(singletons.Contains(new ItemSet(counter: 0, items: new int?[] { 1, null })));
            Assert.IsTrue(singletons.Contains(new ItemSet(counter: 0, items: new int?[] { 1, null })));
        }

        [Test]
        [Ignore("not ready yet")]
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

            //When
            apriori.FindLargeItemSets(dataSet);
        
            
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
