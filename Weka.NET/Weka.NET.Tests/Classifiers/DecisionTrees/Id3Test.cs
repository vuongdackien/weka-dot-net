using NUnit.Framework;
using Weka.NET.Classifiers.DecisionTrees;
using Weka.NET.Core;
using Weka.NET.Classifiers;
namespace Weka.NET.Tests.Classifiers.DecisionTrees
{
    [TestFixture]
    public class Id3Test
    {
        [Test]
        [ExpectedException(typeof(ClassificationException))]
        public void CannotBuildId3ForNumericAttributes()
        {
            //Given
            var dataSetWithNumericAttributeOnly = DataSetBuilder.AnyDataSet()
                .WithNumericAttribute("numeric_attribute")
                    .AddInstance("1")
                        .Build();

            //When
            new Id3Builder().BuildClassifier(dataSetWithNumericAttributeOnly, 0);
        }

        [Test]
        [ExpectedException(typeof(ClassificationException))]
        public void CannotBuildId3IfThereIsMissingAttribute()
        {
            //Given
            var dataSetWithNumericAttributeOnly = DataSetBuilder.AnyDataSet()
                .WithNominalAttribute("nominal_attribute", new []{"some_value"})
                    .AddInstance("some_value")
                    .AddInstance("?")
                        .Build();

            //When
            new Id3Builder().BuildClassifier(dataSetWithNumericAttributeOnly, 0);
        }
    }
}
