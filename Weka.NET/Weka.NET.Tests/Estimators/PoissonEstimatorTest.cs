namespace Weka.NET.Tests.Estimators
{
    using NUnit.Framework;
    using Weka.NET.Estimators;

    [TestFixture]
    public class PoissonEstimatorTest
    {
        [Test]
        public void BuildingPoissonEstimator()
        {
            var estimator = new PoissonEstimator();

            estimator.AddValue(1.4, 1);
            estimator.AddValue(2.2, 1);
            estimator.AddValue(4.3, 1);
            estimator.AddValue(3.7, 1);
            estimator.AddValue(5.6, 1);

            var prob = estimator.GetProbability(3.5);

            Assert.AreEqual(0.19762661980246898d, prob);
        }
    }
}
