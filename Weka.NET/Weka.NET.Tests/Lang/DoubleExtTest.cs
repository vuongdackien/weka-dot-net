namespace Weka.NET.Tests.Lang
{
    using Weka.NET.Lang;
    using NUnit.Framework;
    
    [TestFixture]
    public class DoubleExtTest
    {
        [Test]
        public void GreaterOrEqualsTo_TestingSimpleDifferences()
        {
            var a = .0004;
            var b = .0002;

            Assert.IsTrue(a.GreaterOrEqualsTo(b));

            var c = .0002;
            var d = .0004;

            Assert.IsFalse(c.GreaterOrEqualsTo(d));
        }

        [Test]
        public void CheckingGreaterOrEqualsTo_CanDifferentiateIfValuesAreLessThanSmallConstant()
        {
            var a = .00000001;
            var b = .00000002;

            Assert.IsTrue(a.GreaterOrEqualsTo(b));
            Assert.IsTrue(b.GreaterOrEqualsTo(a));
        }
    }
}
