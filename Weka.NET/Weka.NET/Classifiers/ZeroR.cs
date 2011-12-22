namespace Weka.NET.Classifiers
{
    using Weka.NET.Core;

    public class ZeroRBuilder : IClassifierBuilder<ZeroR>
    {
        public ZeroR BuildClassifier(IDataSet trainingData)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ZeroR : DistributionClassifier
    {
        public ZeroR(Core.Attribute classAttribute, int classIndex)
            : base(classAttribute, classIndex)
        {
        }

        public override double[] DistributionForInstance(Instance instance)
        {



            throw new System.NotImplementedException();
        }
    }
}
