namespace Weka.NET.Estimators
{
    using System;
    using Weka.NET.Core;
    
    public sealed class NormalEstimator : IEstimator
    {
        /// <summary>
        /// The sum of the weights
        /// </summary>
        public double SumOfWeights { get; private set; }

        /// <summary>
        /// The sum of the values seen
        /// </summary>
        public double SumOfValues { get; private set; }

        /// <summary>
        /// The sum of the values squared
        /// </summary>
        public double SumOfValuesSq { get; private set; }

        /// <summary>
        /// The current mean
        /// </summary>
        public double Mean { get; private set; }

        /// <summary>
        /// The current standard deviation
        /// </summary>
        public double StandardDev { get; private set; }

        /// <summary>
        /// The precision of numeric values ( = minimum std dev permitted)
        /// </summary>
        public double Precision { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="precision">
        /// Precision the precision to which numeric values are given. 
        /// For example, if the precision is stated to be 0.1, the values in the interval 
        /// (0.25,0.35] are all treated as 0.3. 
        /// </param>
        public NormalEstimator(double precision)
        {
            Precision = precision;

            StandardDev = Precision / (2 * 3); //Allow at most 3 sd's within one interval
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="weight"></param>
        public void AddValue(double data, double weight)
        {
            if (weight == 0)
            {
                return;
            }

            SumOfWeights += weight;
            SumOfValues += data * weight;
            SumOfValuesSq += data * data * weight;

            if (SumOfWeights > 0)
            {
                Mean = SumOfValues / SumOfWeights;

                double stdDev = Math.Sqrt(Math.Abs(SumOfValuesSq - Mean * SumOfValues) / SumOfWeights);
                
                // If the stdDev ~= 0, we really have no idea of scale yet,
                // so stick with the default. Otherwise...
                if (stdDev > 1e-10)
                {
                    // allow at most 3sd's within one interval
                    StandardDev = Math.Max(Precision / (2 * 3), stdDev);
                }
            }
        }

        public double GetProbability(double data)
        {
            double zLower = (data - Mean - (Precision / 2)) / StandardDev;
            
            double zUpper = (data - Mean + (Precision / 2)) / StandardDev;

            double pLower = Statistics.NormalProbability(zLower);
            
            double pUpper = Statistics.NormalProbability(zUpper);

            return pUpper - pLower;
        }

        public override string ToString()
        {
            return "Normal Distribution. Mean = " + Mean.ToString("#.####")
                + " StandardDev = " + StandardDev.ToString("#.####")
                + " WeightSum = " + SumOfWeights.ToString("#.####")
                + " Precision = " + Precision.ToString("#.####") + "\n";
        }
    }
}
