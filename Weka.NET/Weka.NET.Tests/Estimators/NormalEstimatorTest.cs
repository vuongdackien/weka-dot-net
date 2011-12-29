namespace Weka.NET.Tests.Estimators
{
    using NUnit.Framework;
    using Weka.NET.Estimators;

    [TestFixture]
    public class NormalEstimatorTest
    {
        [Test]
        public void BuildingNormalEstimator()
        {
            var estimator = new NormalEstimator(0.01);

            estimator.AddValue(1.4, 1);
            estimator.AddValue(2.2, 1);
            estimator.AddValue(4.3, 1);
            estimator.AddValue(3.7, 1);
            estimator.AddValue(5.6, 1);

            var prob = estimator.GetProbability(3.5);

            Assert.AreEqual(0.002666729311595506d, prob);
        }
    }
}
