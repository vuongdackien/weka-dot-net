using NUnit.Framework;
using Weka.NET.Core;
using Weka.NET.Filters.Unsupervised.Attribute;
using System.IO;

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
            var dataSet = DataSetBuilder.AnyDataSet().WithRelationName("string_to_numeric_test")
                .WithStringAttribute("Document")
                .AddInstance(new[] { "'Hello World World'" })
                .AddInstance(new[] { "'Hello Hello'" })
                    .Build();

            //When
            var actual = new StringToVector().Filter(dataSet);

            var expected = DataSetBuilder.AnyDataSet().WithRelationName("")
                .Build();
        }
    }
}
