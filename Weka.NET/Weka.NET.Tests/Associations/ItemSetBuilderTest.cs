using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Weka.NET.Core;
using Weka.NET.Associations;
using Weka.NET.Utils;

namespace Weka.NET.Tests.Associations
{
    [TestFixture]
    public class ItemSetBuilderTest
    {
        [Test]
        public void BuildingAllSingletons()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithNominalAttribute
                    (name: "first_attribute", values: new[] { "first0", "first1" })
                .WithNominalAttribute
                    (name: "second_attribute", values: new[] { "second0", "second1" })

                .AddData(new[] { "first0", "second0" })
                .AddData(new[] { "first1", "second1" })
                .AddData(new[] { "first0", "second0" })

                .Build();

            //When
            var builder = new ItemSetBuilder(0);

            var singletons = builder.BuildSingletons(dataSet);

            //then
            Assert.AreEqual(4, singletons.Count);
            Assert.IsTrue(singletons.Contains(new ItemSet(new double?[] { 0, null }, 2)));
            Assert.IsTrue(singletons.Contains(new ItemSet(new double?[] { null, 0 }, 2)));
            Assert.IsTrue(singletons.Contains(new ItemSet(new double?[] { 1, null }, 1)));
            Assert.IsTrue(singletons.Contains(new ItemSet(new double?[] { null, 1 }, 1)));
        }

        [Test]
        public void MergingRightItemAgainstLeftSets()
        {
            //Given
            var dataSet = TestSets.WeatherNominal();

            var builder = new ItemSetBuilder(0);

            var someItemSets = new List<ItemSet> 
                { 
                new ItemSet(new double?[] { null, 0d, null, 1d }, 1)
                , new ItemSet(new double?[] { 0d, null, null, null }, 1)
                };

            var actual = builder.MergeRightAgainstLeft(dataSet, someItemSets[0], someItemSets[1]);

            var expected = new List<ItemSet> 
                { 
                    new ItemSet(new double?[] { 0d, 0d, null, 1d }, 1)
                };

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CombiningItemSets()
        {
            //Given
            var dataSet = TestSets.WeatherNominal();

            var builder = new ItemSetBuilder(0);

            var someItemSets = new List<ItemSet> 
                { 
                new ItemSet(new double?[] { null, 0d, null, 1d }, 1)
                , new ItemSet(new double?[] { 0d, null, null, null }, 1)
                };

            var actual = builder.CombineItemSets(dataSet, someItemSets);

            //then
            Assert.AreEqual(3, actual.Count);
            Assert.IsTrue(actual.Contains(new ItemSet(new double?[] { 0d, null, null, 1d }, 3)));
            Assert.IsTrue(actual.Contains(new ItemSet(new double?[] { 0d, 0d, null, null }, 2)));
            Assert.IsTrue(actual.Contains(new ItemSet(new double?[] { 0d, 0d, null, 1d }, 1)));
        }

        public void PrunningItemSets()
        {
            //Given
            var dataSet = TestSets.WeatherNominal();

            var builder = new ItemSetBuilder(.2);

            //When
            var toPrune = new List<ItemSet>
                {
                    new ItemSet(new double?[] { 0d, null, null, 1d }, 3)
                    ,
                    new ItemSet(new double?[] { 0d, 0d, null, null }, 2)
                    ,
                    new ItemSet(new double?[] { 0d, 0d, null, 1d }, 1)
                };

            var prunned = builder.Prune(toPrune, 3);

            //Then
            Assert.AreEqual(new List<ItemSet> { new ItemSet(new double?[] { 0d, null, null, 1d }, 3) }, prunned);
        }

        [Test]
        public void RecursivelyBuildingAndPrunningItemSets()
        {
            //Given
            var dataSet = TestSets.WeatherNominal();

            var builder = new ItemSetBuilder(.2);

            var actual = builder.BuildItemSets(dataSet);

            //then
            Assert.AreEqual(4, actual.Count);
        }

    }
}
