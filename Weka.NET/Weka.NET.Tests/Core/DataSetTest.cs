namespace Weka.NET.Tests.Core
{
    using NUnit.Framework;
    using Weka.NET.Core;

    [TestFixture]
    public class DataSetTest
    {
        [Test]
        public void CanCheckForMissingValuesForSomeAttribute()
        {
            //When
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithRelationName("some_name")
                .WithNominalAttribute
                    (name: "nominal_attribute", values: new[] { "some_value" })
                .WithNumericAttribute
                    (name: "numeric_attribute")
                .AddInstance(values: new[] { "some_value", "?" })
                .AddInstance(values: new[] { "some_value", "1" })
                    .Build();            
            
            //Then
            Assert.IsFalse(dataSet.ContainsMissingValuesForAttribute(0));

            Assert.IsTrue(dataSet.ContainsMissingValuesForAttribute(1));
        }
    }
}
