namespace Weka.NET.Tests.Core
{
    using Weka.NET.Core;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class DataSetBuilderTest
    {
        [Test]
        public void CanDuplicateDataSet()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithRelationName("some_name")
                .WithNominalAttribute
                    (name: "nominal_attribute", values: new[] { "some_value" })
                .WithNumericAttribute
                    (name: "numeric_attribute")
                .AddInstance(values: new[] { "some_value", "?" })
                .AddInstance(values: new[] { "?", "1" })
                    .Build();

            //When
            var duplicated = new DataSetBuilder().Duplicate(dataSet);

            //Then
            Assert.AreEqual("some_name", duplicated.RelationName);

            Assert.IsTrue(dataSet.Attributes.Count > 0);

            for (int attributeIdx = 0; attributeIdx < dataSet.Attributes.Count; attributeIdx++)
            {
                Assert.AreEqual(dataSet.Attributes[attributeIdx], duplicated.Attributes[attributeIdx]);
            }

            Assert.IsTrue(dataSet.Instances.Count > 0);

            for (int instanceIdx = 0; instanceIdx < dataSet.Instances.Count; instanceIdx++)
            {
                Assert.AreEqual(dataSet.Instances[instanceIdx], duplicated.Instances[instanceIdx]);
            }
        }

        [Test]
        public void SplittingDataSetByAttribute()
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithRelationName("some_name")
                .WithNominalAttribute
                    (name: "first_attribute_0", values: new[] { "first_0", "first_1" })
                .WithNominalAttribute
                    (name: "second_attribute_1", values: new[] { "second_0", "second_1" })

                .AddInstance(values: new[] { "first_0", "second_1" })
                .AddInstance(values: new[] { "first_0", "second_1" })
                .AddInstance(values: new[] { "first_1", "second_1" })

                    .Build();

            //When
            var actual = new DataSetBuilder().SplitDataSet(dataSet, 0);

            //Then
            Assert.AreEqual(2, actual.Count);

            var firstDataSet = DataSetBuilder.AnyDataSet()
                .WithRelationName("some_name")
                .WithNominalAttribute
                    (name: "first_attribute_0", values: new[] { "first_0", "first_1" })
                .WithNominalAttribute
                    (name: "second_attribute_1", values: new[] { "second_0", "second_1" })
                .AddInstance(values: new[] { "first_0", "second_1" })
                .AddInstance(values: new[] { "first_0", "second_1" })
                    .Build();

            var secondDataSet = DataSetBuilder.AnyDataSet()
                .WithRelationName("some_name")
                .WithNominalAttribute
                    (name: "first_attribute_0", values: new[] { "first_0", "first_1" })
                .WithNominalAttribute
                    (name: "second_attribute_1", values: new[] { "second_0", "second_1" })
                .AddInstance(values: new[] { "first_1", "second_1" })
                    .Build();

            Assert.IsTrue(DataSetBuilder.EqualsAttributes(firstDataSet, actual[0]));
            Assert.IsTrue(DataSetBuilder.EqualsInstances(firstDataSet, actual[0]));

            Assert.IsTrue(DataSetBuilder.EqualsAttributes(secondDataSet, actual[1]));
            Assert.IsTrue(DataSetBuilder.EqualsInstances(secondDataSet, actual[1]));
        }
    }
}
