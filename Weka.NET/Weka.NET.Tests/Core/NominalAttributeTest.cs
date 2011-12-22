namespace Weka.NET.Tests.Core
{
    using NUnit.Framework;
    using Weka.NET.Core;

    [TestFixture]
    public class NominalAttributeTest
    {
        [Test]
        public void CanParseMissingValue()
        {
            var attribute = new NominalAttribute(name: "SomeAttribute", values: new[] {"value1", "value2" });

            Assert.AreEqual(Weka.NET.Core.Attribute.DecodedMissingAttribute, attribute.Decode(double.NaN));
        }

        [Test]
        public void CanParseNonMissingValue()
        {
            var attribute = new NominalAttribute(name: "SomeAttribute", values: new[] { "value1", "value2" });

            Assert.AreEqual(attribute.Values[1], attribute.Decode(1d));
        }    
    
    }
}
