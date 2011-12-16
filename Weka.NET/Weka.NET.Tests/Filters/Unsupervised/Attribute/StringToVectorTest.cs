using NUnit.Framework;
using Weka.NET.Core;
using Weka.NET.Filters.Unsupervised.Attribute;

namespace Weka.NET.Tests.Filters.Unsupervised.Attribute
{
    [TestFixture]
    public class StringToVectorTest
    {
        [Test]
        [Ignore("still under work in progress")]
        public void CanTransformStringAttribute()
        {
            //Given
            var dataSetReader = DataSetBuilder.AnyDataSet().WithRelationName("string_to_numeric_test")
                .WithStringAttribute("Document")
                .AddData(new[] { "'Hello World World'" })
                .AddData(new[] { "'Hello Hello'" })
                    .BuildAsStreamReader();

            //When
            var filteredReader = new StringToVector().Filter(dataSetReader);

        }
    }
}
