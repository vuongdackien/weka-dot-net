namespace Weka.NET.Tests.Classifiers.DecisionTrees
{
    using NUnit.Framework;
    using Weka.NET.Core;
    using Weka.NET.Classifiers.DecisionTrees;
    using Weka.NET.Lang;
    
    [TestFixture]
    public class EntropyCalculatorTest
    {
        [Test]
        public void CalculatingInfoGainForDataSetWithVeryLowEntropy()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithRelationName("low_entropy_data_set")
                .WithNominalAttribute
                    (name: "nominal_attribute", values: new[] { "first0", "first1", "first2" })

                .AddInstance(new[] { "first0" })
                .AddInstance(new[] { "first0" })
                .AddInstance(new[] { "first0" })

                    .Build();

            //When
            double infoGain = EntropyCalculator.CalculateInfoGain(dataSet, 0);

            //Then
            Assert.AreEqual(0d, infoGain);
        }

        [Test]
        public void CalculatingInfoGainForDataSetWithVeryHighEntropy()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithRelationName("low_entropy_data_set")
                .WithNominalAttribute
                    (name: "nominal_attribute", values: new[] { "first0", "first1", "first2" })

                .AddInstance(new[] { "first0" })
                .AddInstance(new[] { "first1" })
                .AddInstance(new[] { "first2" })

                    .Build();

            //When
            double infoGain = EntropyCalculator.CalculateInfoGain(dataSet, 0);

            //Then
            System.Console.WriteLine("info gain: " + infoGain);
        }

        [Test]
        public void CalculatingEntropyForDataSetWithVeryLowEntropy()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithRelationName("low_entropy_data_set")
                .WithNominalAttribute
                    (name: "nominal_attribute", values: new[] { "first0", "first1", "first2" })

                .AddInstance(new[] { "first0" })
                .AddInstance(new[] { "first0" })
                .AddInstance(new[] { "first0" })

                    .Build();

            //When
            double entropy = EntropyCalculator.CalculateEntropy(dataSet, 0);

            //Then
         System.Console.WriteLine(entropy);
        }

        [Test]
        public void CalculatingEntropyForDataSetWithVeryHighEntropy()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithRelationName("low_entropy_data_set")
                .WithNominalAttribute
                    (name: "nominal_attribute", values: new[] { "first0", "first1", "first2" })

                .AddInstance(new[] { "first0" })
                .AddInstance(new[] { "first1" })
                .AddInstance(new[] { "first1" })
                .AddInstance(new[] { "first2" })
                .AddInstance(new[] { "first2" })
                .AddInstance(new[] { "first2" })

                    .Build();

            //When
            double actualEntropy = EntropyCalculator.CalculateEntropy(dataSet, 0);

            System.Console.WriteLine("Actual entropy: " + actualEntropy);

            //Then
            double expectedEntropy = 0;

            int classCountForFirst0Value = 1;
            int classCountForFirst1Value = 2;
            int classCountForFirst2Value = 3;

            expectedEntropy -= classCountForFirst0Value * Stats.Log_2(classCountForFirst0Value);
            expectedEntropy -= classCountForFirst1Value * Stats.Log_2(classCountForFirst1Value);
            expectedEntropy -= classCountForFirst2Value * Stats.Log_2(classCountForFirst2Value);

            expectedEntropy /= (double)dataSet.Count;

            expectedEntropy += Stats.Log_2(dataSet.Count);

            Assert.AreEqual(expectedEntropy, actualEntropy);
        }

    }
}
