namespace Weka.NET.Tests.Lang
{
    using NUnit.Framework;
    using Weka.NET.Lang;
    using System;
    
    [TestFixture]
    public class StatsTest
    {
        [Test]
        public void CalculatingLog2()
        {
            for (int i = -100; i < 100; i++)
            {
                var random = new Random();

                double num = i + random.NextDouble();

                Assert.AreEqual(Math.Log(num) / Stats.log2, Stats.Log_2(num));
            }
        }
    }
}
