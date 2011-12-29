namespace Weka.NET.Estimators
{
    using System;
    using Weka.NET.Core;

    public class PoissonEstimator : IEstimator
    {
        /// <summary>
        /// The number of values seen.
        /// </summary>
        public double NumValues { get; private set; }

        /// <summary>
        /// The sum of the values seen.
        /// </summary>
        public double SumOfValues { get; private set; }

        /// <summary>
        /// The average number of times an event occurs in an interval.
        /// </summary>
        public double Lambda { get; private set; }

        public void AddValue(double data, double weight)
        {
            NumValues += weight;

            SumOfValues += data * weight;
            
            if (NumValues != 0)
            {
                Lambda = SumOfValues / NumValues;
            }
        }

        public double GetProbability(double x)
        {
            return Math.Exp(-Lambda + (x * Math.Log(Lambda)) - Statistics.LogFac(x));
        }
    }
}
