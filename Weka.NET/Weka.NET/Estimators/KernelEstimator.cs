namespace Weka.NET.Estimators
{
    using System.Collections.Generic;
    
    public class KernelEstimator : IEstimator
    {
        /// <summary>
        /// Vector containing all of the values seen
        /// </summary>
        public IList<double> Values { get; private set; }

        /// <summary>
        /// Vector containing the associated weights
        /// </summary>
        public IList<double> Weights { get; private set; }

        /// <summary>
        /// Number of values stored in Weights and Values so far
        /// </summary>
        public int NumValues { get { return Values.Count; } }

        /// <summary>
        /// The sum of the weights so far
        /// </summary>
        public double SumOfWeights { get; private set; }

        /// <summary>
        /// The standard deviation
        /// </summary>
        public double StandardDev { get; private set; }

        /// <summary>
        /// The precision of data values
        /// </summary>
        public double Precision { get; private set; }

        /// <summary>
        /// Whether we can optimise the kernel summation
        /// </summary>
        public bool AllWeightsOne { get; private set; }

        /// <summary>
        /// Default maximum percentage error permitted in probability calculations
        /// </summary>
        public const double MAX_ERROR = 0.01;

        public void AddValue(double data, double weight)
        {
            throw new System.NotImplementedException();
        }

        public void GetProbability(double value)
        {
            throw new System.NotImplementedException();
        }

        internal int FindNearestValue(double key)
        {
            int low = 0;
            
            int high = NumValues;
            
            int middle = 0;
            
            while (low < high)
            {
                middle = (low + high) / 2;
                
                double current = Values[middle];
                
                if (current == key)
                {
                    return middle;
                }

                if (current > key)
                {
                    high = middle;
                }

                else if (current < key)
                {
                    low = middle + 1;
                }
            }

            return low;
        }

    }
}
