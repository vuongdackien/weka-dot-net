namespace Weka.NET.Classifiers.DecisionTrees
{
    using Weka.NET.Core;
    using Weka.NET.Lang;
    
    public static class EntropyCalculator
    {
        public static double CalculateInfoGain(IDataSet dataSet, int attributeIndex)
        {
            double infoGain = CalculateEntropy(dataSet, attributeIndex);

            System.Console.WriteLine("original infogain: " + infoGain);

            var splitDataSets = new DataSetBuilder().SplitDataSet(dataSet, attributeIndex);

            System.Console.WriteLine("Splitted data set: " + splitDataSets.Count);

            foreach(var splitDataSet in splitDataSets)
            {
                if (splitDataSet.Count == 0)
                {
                    System.Console.WriteLine("I'm here");
                    continue;
                }
                
                System.Console.WriteLine("((double) splitDataSet.Count) / ((double) dataSet.Count): " + ((double) splitDataSet.Count) / ((double) dataSet.Count));

                System.Console.WriteLine("CalculateEntropy(splitDataSet, attributeIndex)" + CalculateEntropy(splitDataSet, attributeIndex));

                double delta = ( ((double) splitDataSet.Count) / ((double) dataSet.Count)  ) * CalculateEntropy(splitDataSet, attributeIndex);

                System.Console.WriteLine("delta: " + delta);

                infoGain -= delta;

                System.Console.WriteLine("infogain: " + infoGain);

            }

		    return infoGain; 
        }

        /// <summary>
        /// Calculates the entropy of the data's distribution for the attributed
        /// specified as argument.
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="attributeIndex"></param>
        /// <returns></returns>
        public static double CalculateEntropy(IDataSet dataSet, int attributeIndex)
        {
            var attribute = dataSet.Attributes[attributeIndex];

            var classCounts = new double[
                attribute is NominalAttribute ?
                    ((NominalAttribute)attribute).Values.Length
                    : 1
                ];

            foreach (var instance in dataSet.Instances)
            {
                if (instance.IsMissing(attributeIndex))
                {
                    throw new System.ArgumentException("Can't calculate entropy for missing values");
                }

                classCounts[(int) instance.Values[attributeIndex]]++;
            }

            double entropy = 0;

            for (int i = 0; i < classCounts.Length; i++)
            {
                if (classCounts[i] > 0)
                {
                    entropy -= classCounts[i] * Stats.Log_2(classCounts[i]);
                }
            }

            entropy /= (double)dataSet.Count;

            return entropy + Stats.Log_2(dataSet.Count);
        }
    }
}
