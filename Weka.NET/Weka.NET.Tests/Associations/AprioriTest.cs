using NUnit.Framework;
using System;
using Weka.NET.Core;
using Weka.NET.Associations;
using System.Collections.Generic;

namespace Weka.NET.Tests.Associations
{
    [TestFixture]
    public class AprioriTest
    {
        Apriori apriori;

        [SetUp]
        public void SetUpApriori()
        {
            
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotAcceptStringAttribute()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithStringAttribute(name: "string_attribute")
                .Build();

            //When
           // apriori.BuildAssociationRules(dataSet: dataSet, maxNumRules: 10);

            //Then Expect ArgumentException
        }



        [Test]
        public void UpdatingCounters()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithNominalAttribute
                    (name: "first_attribute", values: new[] { "first0", "first1" })
                .WithNominalAttribute
                    (name: "second_attribute", values: new[] { "second0", "second1" })

                .AddData(values: new[] { "first0", "second0" })
                .AddData(values: new[] { "first0", "second1" })
                .AddData(values: new[] { "first0", "second1" })
                .AddData(values: new[] { "first0", "second1" })
                .AddData(values: new[] { "first1", "second1" })
                .AddData(values: new[] { "first1", "second1" })

                .Build();

            var expectedCounts = new Dictionary<ItemSet, int>();

            expectedCounts[new ItemSet(new double?[] { null, 0 })] = 1;
            expectedCounts[new ItemSet(new double?[] { null, 1 })] = 5;
            expectedCounts[new ItemSet(new double?[] { 0, null })] = 4;
            expectedCounts[new ItemSet(new double?[] { 1, null })] = 2;
            expectedCounts[new ItemSet(new double?[] { 0, 0 })] = 1;
            expectedCounts[new ItemSet(new double?[] { 0, 1 })] = 3;
            expectedCounts[new ItemSet(new double?[] { 1, 1 })] = 2;

            //When
            var actualCounts = new Dictionary<ItemSet, int>();

           // apriori.UpdateCounts(expectedCounts.Keys, actualCounts, dataSet);

            //Then
            Assert.AreEqual(expectedCounts, actualCounts);
        }
    }
}
