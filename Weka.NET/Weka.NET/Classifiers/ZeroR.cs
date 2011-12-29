namespace Weka.NET.Classifiers
{
    using Weka.NET.Core;
    using Weka.NET.Lang;
    using Weka.NET.Utils;

    [OptionArgument('t', "Set the training data")]
    public class ZeroRBuilder : IClassifierBuilder<ZeroR>
    {
        public ZeroR BuildClassifier(IDataSet trainingData, int classAttributeIndex)
        {
            Core.Attribute classAttribute = trainingData.Attributes[classAttributeIndex];

            if (classAttribute is NominalAttribute)
            {
                return BuildClassifierForNominalAttribute(trainingData, classAttributeIndex);
            }

            if (classAttribute is NumericAttribute)
            {
                return BuildClassifierForNumericAttribute(trainingData, classAttributeIndex);
            }

            throw new ClassificationException("ZeroR can only handle nominal and numeric class attributes.");
        }

        private ZeroR BuildClassifierForNumericAttribute(IDataSet trainingData, int classAttributeIndex)
        {
            double classValue = 0;

            double sumsOfWeights = 0;

            foreach (var instance in trainingData.Instances)
            {
                classValue += instance.Weight * instance.Values[classAttributeIndex];

                sumsOfWeights += instance.Weight;
            }

            if (classValue.GreaterOrEqualsTo(0))
            {
                classValue /= sumsOfWeights;
            }

            return new ZeroR(trainingData.Attributes[classAttributeIndex], classAttributeIndex, classValue, new double[]{classValue});
        }

        private ZeroR BuildClassifierForNominalAttribute(IDataSet trainingData, int classAttributeIndex)
        {
            var classAttribute = trainingData.Attributes[classAttributeIndex] as NominalAttribute;

            double[] counts = new double[classAttribute.Values.Length];

            for (int i = 0; i < counts.Length; i++)
            {
                counts[i] = 1;
            }

            foreach (var instance in trainingData.Instances)
            {
                if (instance.IsMissing(classAttributeIndex))
                {
                    continue;
                }

                counts[(int) instance[classAttributeIndex]] += instance.Weight;
            }

            var classValue = counts.MaxIndex();

            var normalizedCounts = counts.Normalize();

            return new ZeroR(trainingData.Attributes[classAttributeIndex], classAttributeIndex, classValue, normalizedCounts);
        }
    }

    public class ZeroR : DistributionClassifier
    {
        public double ClassValue { get; private set; }

        public double[] NormalizedCounts { get; private set; }

        public ZeroR(Attribute classAttribute, int classAttributeIndex, double classValue, double[] normalizedCounts)
            : base(classAttribute, classAttributeIndex)
        {
            ClassValue = classValue;
            NormalizedCounts = (double[]) normalizedCounts.Clone();
        }

        public override double[] DistributionForInstance(Instance instance)
        {
            return NormalizedCounts;
        }

        public override double ClassifyInstance(Instance instance)
        {
            return ClassValue;
        }
    }
}
