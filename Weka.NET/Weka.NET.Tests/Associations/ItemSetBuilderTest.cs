
using NUnit.Framework;
using Weka.NET.Core;
using Weka.NET.Associations;
using System;
using System.Collections.Generic;
using Weka.NET.Utils;

namespace Weka.NET.Tests.Associations
{
    [TestFixture]
    public class ItemSetSupportsTest
    {
        [Test]
        public void CanCountItemSetSupport()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithNominalAttribute
                    (name: "first_attribute", values: new[] { "first0", "first1" })
                .WithNominalAttribute
                    (name: "second_attribute", values: new[] { "second0", "second1" })

                .AddData(new[] { "first0", "second0" })
                .AddData(new[] { "first0", "second0" })
                .AddData(new[] { "first0", "second0" })

                .Build();

            //When
            var supports = new ItemSetSupports(dataSet);

            supports.AddItemSets(new[] { new ItemSet(new double?[] { null, 0d }) });

            //Then
            Assert.AreEqual(3, supports[new ItemSet(new double?[] { null, 0d })]);
        }
    }

    [TestFixture]
    public class ItemSetBuilderStuffTest
    {
        [Test]
        public void FindingLargeItemSetsByPrunning()
        { 
            //Given
            var dataSet = TestSets.WeatherNominal();

            //And
            var builder = new ItemSetBuilder(0.1);

            var singletons = builder.BuildSingletons(dataSet.Attributes);

            var stuff = new ItemSetBuilder.ItemSetBuilderStuff(dataSet, 6);

            stuff.AddAll(singletons);

            //When
            builder.FindSets(1, stuff);

            Console.WriteLine(stuff.ItemSets);
        }
        
        [Test]
        public void FindingLargeItemSetsWithoutPrunning()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithNominalAttribute
                    (name: "first_attribute", values: new[] { "first0", "first1" })
                .WithNominalAttribute
                    (name: "second_attribute", values: new[] { "second0", "second1" })
                .WithNominalAttribute
                    (name: "third_attribute", values: new[] { "third0", "third1" })

                .Build();

            //And
            var builder = new ItemSetBuilder(0);

            var singletons = builder.BuildSingletons(dataSet.Attributes);

            var stuff = new ItemSetBuilder.ItemSetBuilderStuff(dataSet, 0);

            stuff.AddAll(singletons);

            //When
            builder.FindSets(1, stuff);

            Console.WriteLine(stuff.ItemSets);
        
        }

        [Test]
        public void CombiningLargestItemSet()
        {
            var stuff = new ItemSetBuilder(.01);

            var singletons = new List<ItemSet> 
                { 
                new ItemSet(new double?[] { null, 0d, null, 1d })
                , new ItemSet(new double?[] { 0d, null, null, null })
                };

            var actual = stuff.CombineItemSets(items: singletons, i: 0, j: 1, size: 1);

            var expected = new List<ItemSet> 
                { 
                    new ItemSet(new double?[] { 0d, 0d, null, 1d })
                    , new ItemSet(new double?[] { 0d, 0d, null, null })
                    , new ItemSet(new double?[] { 0d, null, null, 1d })
                };

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CombiningTwoSingletons()
        {
            var stuff = new ItemSetBuilder(.01);

            var singletons = new List<ItemSet> 
                { 
                new ItemSet(new double?[] { null, 0d })
                , new ItemSet(new double?[] { 0d, null })
                };

            var actual = stuff.CombineItemSets(items:singletons, i:0, j:1, size:1);

            var expected = new List<ItemSet> 
                { 
                    new ItemSet(new double?[] { 0d, 0d })
                    , new ItemSet(new double?[] { 0d, 0d })
                }; 

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanCountItemSetSupport()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithNominalAttribute
                    (name: "first_attribute", values: new[] { "first0", "first1" })
                .WithNominalAttribute
                    (name: "second_attribute", values: new[] { "second0", "second1" })

                .AddData(new[] { "first0", "second0" })
                .AddData(new[] { "first0", "second0" })
                .AddData(new[] { "first0", "second0" })

                .Build();

            //And
            var stuff = new ItemSetBuilder.ItemSetBuilderStuff(dataSet, 3);

            //When
            stuff.AddAll(new[] { new ItemSet(new double?[] { null, 0d }) });

            //Then
            Assert.AreEqual(3, stuff.ItemSets.Counts[new ItemSet(new double?[] { null, 0d })]);
        }
    }

    [TestFixture]
    public class ItemSetBuilderTest
    {

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

            var builder = new ItemSetBuilder(minSupport: .1);

            //When
            var singletons = builder.BuildSingletons(dataSet.Attributes);

            //Then
            Assert.AreEqual(4, singletons.Count);
            Assert.IsTrue(singletons.Contains(new ItemSet(items: new double?[] { null, 0 })));
            Assert.IsTrue(singletons.Contains(new ItemSet(items: new double?[] { null, 1 })));
            Assert.IsTrue(singletons.Contains(new ItemSet(items: new double?[] { 1, null })));
            Assert.IsTrue(singletons.Contains(new ItemSet(items: new double?[] { 1, null })));
        }
    }
}
