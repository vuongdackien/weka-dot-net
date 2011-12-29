using NUnit.Framework;
using Weka.NET.Estimators;
namespace Weka.NET.Tests.Estimators
{
    [TestFixture]
    public class DiscreteEstimatorTest
    {
        [Test]
        public void CanBuildDiscreteEstimator()
        {
            var estimator = new DiscreteEstimator(3, true);

            estimator.AddValue(0d, 1d);
            estimator.AddValue(0d, 1d);
            estimator.AddValue(1d, 1d);
            estimator.AddValue(1d, 1d);
            estimator.AddValue(2d, 1d);
            estimator.AddValue(2d, 1d);
            estimator.AddValue(2d, 1d);

            Assert.AreEqual(0.29999999999999999d, estimator.GetProbability(1d));
        }
    }
}
