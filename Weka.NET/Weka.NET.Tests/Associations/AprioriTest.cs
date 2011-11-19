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
        public void CannotAcceptStringAttribute()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithStringAttribute(name: "string_attribute")
                .Build();

            //When
            new Apriori(.3).BuildAssociationRules(dataSet);

            //Then Expect ArgumentException
        }

        [Test]
        public void CanGenerateAllRulesWithOneItemInTheConsequence()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithNominalAttribute
                    (name: "first_attribute", values: new[] { "first0", "first1" })
                .WithNominalAttribute
                    (name: "second_attribute", values: new[] { "second0", "second1" })
                .WithNominalAttribute
                    (name: "third_attribute", values: new[] { "third0", "third1" })

                .AddData(values: new[] { "first0", "second0", "third0" })
                .AddData(values: new[] { "first0", "second1", "third0" })
                .AddData(values: new[] { "first0", "second1", "third1" })
                .AddData(values: new[] { "first0", "second1", "third1" })
                .AddData(values: new[] { "first1", "second1", "third1" })
                .AddData(values: new[] { "first1", "second1", "third1" })

                .Build();

            //When
            var apriori = new Apriori(.3);

            var actualRules = apriori.GenerateAllRulesWithOneItemInTheConsequence(new ItemSet(new int?[] { 0, 1, 0 }), dataSet);

            Console.WriteLine(actualRules[0].ToString());
            Console.WriteLine(actualRules[1].ToString());
            Console.WriteLine(actualRules[2].ToString());

            //Then
            var expectedRules = new List<AssociationRule>
                {
                    new AssociationRule(premise:new ItemSet(new int?[]{null, 1, 0}), premiseCount:1, consequence:new ItemSet(new int?[]{0, null, null}), consequenceCount:4)
                    , new AssociationRule(premise:new ItemSet(new int?[]{0, null, 0}), premiseCount:2, consequence:new ItemSet(new int?[]{null, 1, null}), consequenceCount:5)
                    , new AssociationRule(premise:new ItemSet(new int?[]{0, 1, null}), premiseCount:3, consequence:new ItemSet(new int?[]{null, null, 0}), consequenceCount:2)
                };

            Assert.AreEqual(expectedRules, actualRules);
        }

        [Test]
        public void CanFindLargeItemSets()
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

            //When
            var apriori = new Apriori(1);

            apriori.FindLargeItemSets(dataSet);
        }

        [Test]
        public void CanMergeSimilarItemsWithSizeLessThanLength()
        {
            //Given
            var itemSets = new List<ItemSet>
                {
                    new ItemSet(new int?[] { null, 0, 1 })
                    , new ItemSet(new int?[] { null, 0, null })
                };


            //When
            var apriori = new Apriori(1);

            var merged = apriori.MergeAllItemSets(itemSets, 1);

            //Then
            Assert.AreEqual(new List<ItemSet> { new ItemSet(new int?[] { null, 0, 1 }) }, merged);
        }

        
        [Test]
        public void CanMergeSimilarItems()
        {
            //Given
            var itemSets = new List<ItemSet>
                {
                    new ItemSet(new int?[] { null, 0, 1 })
                    , new ItemSet(new int?[] { null, 0, 1 })
                };


            //When
            var apriori = new Apriori(1);

            var merged = apriori.MergeAllItemSets(itemSets, 2);
        
            //Then
            Assert.AreEqual(new List<ItemSet> { new ItemSet(new int?[] { null, 0, 1 })  }, merged);
        }

        [Test]
        public void CanPruneItemSets()
        {
            //Given
            var toPrune = new List<ItemSet>
                {
                    new ItemSet(new int?[] { 0, 1, 0 })
                };

            var kMinusOne = new List<ItemSet>
                {
                    new ItemSet(new int?[] { null, 1, 0 }) 
                    , new ItemSet(new int?[] { 0, null, 0 }) 
                    , new ItemSet(new int?[] { 0, 1, null })
                };

            //When
            var apriori = new Apriori(1);

            var pruned = apriori.PruneItemSets(toPrune, kMinusOne);
        
            //Then
            Assert.AreEqual(toPrune, pruned);
        }

        [Test]
        public void DeletingItemSetsWithLowSupport()
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

            var itemSets = new Dictionary<ItemSet, int>();

            itemSets[new ItemSet(new int?[] { null, 0 })] = 1;
            itemSets[new ItemSet(new int?[] { null, 1 })] = 5;
            itemSets[new ItemSet(new int?[] { 0, null })] = 4;
            itemSets[new ItemSet(new int?[] { 1, null })] = 2;
            itemSets[new ItemSet(new int?[] { 0, 0 })] = 1;
            itemSets[new ItemSet(new int?[] { 0, 1 })] = 3;
            itemSets[new ItemSet(new int?[] { 1, 1 })] = 2;

            //When
            var apriori = new Apriori(1);
            apriori.UpdateCounts(dataSet, itemSets.Keys);

            //Then
            var actual = apriori.DeleteItemSets(itemSets.Keys.ToList(), 3);

            var expected = new List<ItemSet>{
                new ItemSet(new int?[] { null, 1 })
                , new ItemSet(new int?[] { 0, null })
                , new ItemSet(new int?[] { 0, 1 }) };

            Assert.AreEqual(expected, actual);
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

            expectedCounts[new ItemSet(new int?[] { null, 0 })] = 1;
            expectedCounts[new ItemSet(new int?[] { null, 1 })] = 5;
            expectedCounts[new ItemSet(new int?[] { 0, null })] = 4;
            expectedCounts[new ItemSet(new int?[] { 1, null })] = 2;
            expectedCounts[new ItemSet(new int?[] { 0, 0 })] = 1;
            expectedCounts[new ItemSet(new int?[] { 0, 1 })] = 3;
            expectedCounts[new ItemSet(new int?[] { 1, 1 })] = 2;

            //When
            var apriori = new Apriori(1);
            apriori.UpdateCounts(dataSet, expectedCounts.Keys);

            //Then
            Assert.AreEqual(expectedCounts, apriori.ItemSetCounts);
        }

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
            Assert.IsTrue(singletons.Contains(new ItemSet(items: new int?[] { null, 0 })));
            Assert.IsTrue(singletons.Contains(new ItemSet(items: new int?[] { null, 1 })));
            Assert.IsTrue(singletons.Contains(new ItemSet(items: new int?[] { 1, null })));
            Assert.IsTrue(singletons.Contains(new ItemSet(items: new int?[] { 1, null })));
        }
    
    }
}
