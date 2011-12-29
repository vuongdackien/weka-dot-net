namespace Weka.NET.Clusterers
{
    using Weka.NET.Utils;
    using Weka.NET.Core;

    [OptionArgument('t', "Set the training data")]
    public class EMBuilder
    {
        public const int DefaultMaxIterations = 100;

        public const int DefaultRSeed = 100;

        public const int DefaultNumClusters = -1;

        public EM BuildClusterer(IDataSet dataSet)
        {
            return new EM();
        }
    }

    public class EM
    {
    }
}
