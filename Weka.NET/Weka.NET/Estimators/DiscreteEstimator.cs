namespace Weka.NET.Estimators
{
    public class DiscreteEstimator : IEstimator
    {
        /// <summary>
        /// Hold the counts
        /// </summary>
        public double[] Counts { get; private set; }

        /// <summary>
        /// Hold the sum of counts
        /// </summary>
        public double SumOfCounts { get; private set; }

        public DiscreteEstimator(int numSymbols, bool laplace)
        {
            Counts = new double[numSymbols];
            
            SumOfCounts = 0;
            
            if (laplace)
            {
                for (int i = 0; i < numSymbols; i++)
                {
                    Counts[i] = 1;
                }
                SumOfCounts = (double)numSymbols;
            }
        }

        public void AddValue(double data, double weight)
        {
            Counts[(int)data] += weight;
            
            SumOfCounts += weight;
        }

        public double GetProbability(double data)
        {
            if (SumOfCounts == 0)
            {
                return 0;
            }

            return (double) Counts[(int)data] / SumOfCounts;        
        }

    }
}
