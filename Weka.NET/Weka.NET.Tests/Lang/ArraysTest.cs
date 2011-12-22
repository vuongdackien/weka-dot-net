namespace Weka.NET.Tests.Lang
{
    using Weka.NET.Lang;
    using NUnit.Framework;

    [TestFixture]
    public class ArraysTest
    {
        [Test]
        public void FindingTheMaxArrayIndex()
        {
            double[] array = new double[] { 1d, 3d, 1d };

            Assert.AreEqual(1, array.MaxIndex());
        }
    }
}
