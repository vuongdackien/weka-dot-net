namespace Weka.NET.Tests.Associations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using Weka.NET.Associations;
    using Weka.NET.Core;
    using Weka.NET.Utils;
    
    [TestFixture]
    public class ItemSetBuilderTest
    {
        [Test]
        public void RecursivelyBuildingAndPrunningItemSets()
        {
            //Given
            var dataSet = TestSets.BooleanDataSet();

            var builder = new ItemSetBuilder(.75);

            var actual = builder.BuildItemSets(dataSet);

            //then
            Assert.AreEqual(5, actual.Count);

            Assert.AreEqual(4, actual[new ItemSet(new double?[] { 0, null, null, null })]);

            Assert.AreEqual(3, actual[new ItemSet(new double?[] { null, 0, null, null })]);

            Assert.AreEqual(3, actual[new ItemSet(new double?[] { null, null, 0, null })]);

            Assert.AreEqual(3, actual[new ItemSet(new double?[] { 0, 0, null, null })]);

            Assert.AreEqual(3, actual[new ItemSet(new double?[] { 0, null, 0, null })]);
        }

        [Test]
        public void BuildingAndPrunningSingletons()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithNominalAttribute
                    (name: "first_attribute", values: new[] { "first0", "first1" })
                .WithNominalAttribute
                    (name: "second_attribute", values: new[] { "second0", "second1" })

                .AddInstance(new[] { "first0", "second0" })
                .AddInstance(new[] { "first1", "second1" })
                .AddInstance(new[] { "first0", "second0" })

                .Build();

            //When
            var builder = new ItemSetBuilder(0);

            var prunned = builder.BuildAndPruneSingletons(dataSet, 2);

            //Then
            Assert.AreEqual(2, prunned.Count);
            Assert.AreEqual(2, prunned[new ItemSet(0d, null)]);
            Assert.AreEqual(2, prunned[new ItemSet(null, 0d)]);
        }

        [Test]
        public void BuildingAllSingletonItems()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithNominalAttribute
                    (name: "first_attribute", values: new[] { "first0", "first1" })
                .WithNominalAttribute
                    (name: "second_attribute", values: new[] { "second0", "second1" })

                .AddInstance(new[] { "first0", "second0" })
                .AddInstance(new[] { "first1", "second1" })
                .AddInstance(new[] { "first0", "second0" })

                .Build();

            //When
            var builder = new ItemSetBuilder(0);

            var singletons = builder.BuildSingletonItems(dataSet);

            //then
            Assert.AreEqual(4, singletons.Count);
            Assert.IsTrue(singletons.Contains(new ItemSet(0, null)));
            Assert.IsTrue(singletons.Contains(new ItemSet(null, 0)));
            Assert.IsTrue(singletons.Contains(new ItemSet(1, null)));
            Assert.IsTrue(singletons.Contains(new ItemSet(null, 1)));
        }

        [Test]
        public void CanCalculateSupportForItems()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithNominalAttribute
                    (name: "first_attribute", values: new[] { "first0", "first1" })
                .WithNominalAttribute
                    (name: "second_attribute", values: new[] { "second0", "second1" })

                .AddInstance(new[] { "first0", "second0" })
                .AddInstance(new[] { "first1", "second1" })
                .AddInstance(new[] { "first0", "second0" })

                .Build();

            var items = new ItemSet(0, 0);

            //When
            var actualCount = new ItemSetBuilder(0).CalculateSupport(items, dataSet);

            //Then
            Assert.AreEqual(2, actualCount);
        }
    }

    
    
}
