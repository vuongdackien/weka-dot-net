namespace Weka.NET.Tests.Classifiers
{
    using Weka.NET.Classifiers;
    using NUnit.Framework;
    using Weka.NET.Core;
    using System.Collections.Generic;

    [TestFixture]
    public class ZeroRTest
    {
        [Test]
        public void GeneratingZeroRForNominalClassAttribute()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithNominalAttribute
                    (name: "nominal_attribute", values: new[] { "first0", "first1", "first2" })

                .AddInstance(new[] { "first0" })
                .AddInstance(new[] { "first1" })
                .AddInstance(new[] { "first1" })
                .AddInstance(new[] { "first2" })
                
                    .Build();

            var zeroR = new ZeroRBuilder().BuildClassifier(trainingData: dataSet, classAttributeIndex: 0);

            //When
            var predicted = zeroR.ClassifyInstance(new Instance(values: new List<double>{1d}));

            //Then
            Assert.AreEqual(1d, predicted);
        }

        [Test]
        public void GeneratingZeroRForNumericClassAttribute()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithNumericAttribute("numeric_attribute")
                .AddInstance("1")
                .AddInstance("2.5")
                .AddInstance("2.5")
                .AddInstance("5")
                    .Build();

            var zeroR = new ZeroRBuilder().BuildClassifier(trainingData: dataSet, classAttributeIndex: 0);

            //When
            var predicted = zeroR.ClassifyInstance(new Instance(values: new List<double> { 9d }));

            //Then
            Assert.AreEqual(2.75d, predicted);
        }

        [Test]
        public void ZeroRClassifyNumericAttributeByCalculatingTheWeightedClassValue()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithNominalAttribute
                    (name: "nominal_attribute", values: new[] { "first0", "first1", "first2" })

                .AddInstance(new[] { "first0" })
                .AddInstance(new[] { "first1" })
                .AddWeightedInstance(weight: 7, values: new[] { "first2" })

                    .Build();

            var zeroR = new ZeroRBuilder().BuildClassifier(trainingData: dataSet, classAttributeIndex: 0);

            //When
            var predicted = zeroR.ClassifyInstance(new Instance(values: new List<double> { 1d }));

            //Then
            Assert.AreEqual(2d, predicted);
        }

        [Test]
        public void ZeroRClassifyNominalAttributeByFindingTheAttributeValueWithMoreOccurrences()
        {
        }


    }
}
