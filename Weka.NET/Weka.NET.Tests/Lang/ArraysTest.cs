namespace Weka.NET.Tests.Lang
{
    using Weka.NET.Lang;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    public class ArraysTest
    {
        [Test]
        public void FindingTheMaxArrayIndex()
        {
            double[] array = new []{ 1d, 3d, 1d };

            Assert.AreEqual(1, array.MaxIndex());
        }

        [Test]
        public void NormalizingDoubles()
        {
            //Given
            double[] array = new[] {1.2, 2.3, 1.4};

            //When
            double[] normalized = array.Normalize();

            //Then
            double[] expected = new double[3];

            double sum = array.Sum();

            for (int i = 0; i < array.Length; i++)
            {
                expected[i] = array[i] / sum;
            }

            Assert.IsTrue(expected.ArrayEquals(normalized));
        }
    }
}
