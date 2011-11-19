using Weka.NET.Lang;
using NUnit.Framework;

namespace Weka.NET.Tests.Lang
{
    [TestFixture]
    public class DoubleExtTest
    {
        [Test]
        public void GreaterOrEqualsTo_SimpleDiff()
        {
            var a = .0004;

            var b = .0002;

            Assert.IsTrue(a.GreaterOrEqualsTo(b));
        }

        [Test]
        public void CheckingGreaterOrEqualsTo()
        {
            var a = .0002;

            var b = .0004;

            Assert.IsFalse(a.GreaterOrEqualsTo(b));
        }
    }
}
