namespace Weka.NET.Classifiers
{
    using Weka.NET.Core;
    using Weka.NET.Lang;

    public abstract class DistributionClassifier : IClassifier
    {
        public Core.Attribute ClassAttribute { get; private set; }

        public int ClassAttributeIndex { get; private set; }

        public DistributionClassifier(Core.Attribute classAttribute, int classAttributeIndex)
        {
            ClassAttribute = classAttribute;
            ClassAttributeIndex = classAttributeIndex;
        }

        public abstract double[] DistributionForInstance(Instance instance);

        public virtual double ClassifyInstance(Instance instance)
        {
            double[] distributions = DistributionForInstance(instance);

            if (null == distributions)
            {
                throw new ClassificationException("Null distribution predicted");
            }

            if (ClassAttribute is NumericAttribute)
            {
                return distributions[0];
            }

            if (ClassAttribute is NominalAttribute)
            {
                int maxIdx = distributions.MaxIndex();

                return distributions[maxIdx] > 0 ? distributions[maxIdx] : Core.Attribute.EncodedMissingValue;
            }

            throw new ClassificationException("Not supported ClassAttribute [" + ClassAttribute + "]");
        }
    }
}
