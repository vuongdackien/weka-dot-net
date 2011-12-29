namespace Weka.NET.Classifiers.DecisionTrees
{
    using System.Collections.Generic;
    using Weka.NET.Core;
    using Weka.NET.Lang;

    /// <summary>
    /// Class implementing an Id3 decision tree classifier.
    /// 
    /// References:
    /// 1. R. Quinlan (1986). 
    /// Induction of decision trees.
    /// Machine Learning. Vol.1, No.1, pp. 81-106.
    /// 
    /// 2. This class is based on original Weka's Id3 by Eibe Frank
    /// </summary>
    public class Id3Builder : IClassifierBuilder<Id3>
    {
        public Id3 BuildClassifier(IDataSet trainingData, int classAttributeIndex)
        {
            if (false == trainingData.ContainsNominalAttributesOnly())
            {
                throw new ClassificationException("Id3 only accept Nominal attributes");
            }

            if (trainingData.ContainsMissingValuesForAttribute(classAttributeIndex)) //only class attribute can't contain missing
            {
                throw new ClassificationException("Id3 only accept Nominal attributes");
            }

            var duplicatedTrainingData = new DataSetBuilder().DuplicateRemovingMissing(trainingData); //duplicate by removing missing

            return MakeTree(duplicatedTrainingData, classAttributeIndex, new List<Id3>());
        }

        Id3 MakeTree(IDataSet trainingData, int classAttributeIndex, IList<Id3> successors)
        {
            var classAttribute = trainingData.Attributes[classAttributeIndex] as NominalAttribute;

            //Check if no instances have reached this node.
            if (trainingData.Count == 0)
            {
                return new Id3(classAttribute
                    , classAttributeIndex
                    , Instance.MissingValue
                    , new double[classAttribute.Values.Length]
                    );
            }

            // Compute attribute with maximum information gain.
            double[] infoGains = new double[trainingData.Attributes.Count];

            for (int i = 0; i < trainingData.Attributes.Count; i++)
            {
                infoGains[i] = EntropyCalculator.CalculateInfoGain(trainingData, i);
            }

            int maxInfoGainAttributeIdx = infoGains.MaxIndex();

            var maxInfoGainAttribute = trainingData.Attributes[maxInfoGainAttributeIdx] as NominalAttribute;

            // Make leaf if information gain is zero.
            // Otherwise create successors.
            if (DoubleExt.GreaterOrEqualsTo(infoGains[maxInfoGainAttributeIdx], 0d))
            {
                Core.Attribute SplitAttribute = null;

                var distribution = new double[classAttribute.Values.Length];

                foreach(var instance in trainingData.Instances)
                {
                    distribution[(int) instance[classAttributeIndex]]++;
                }

                var normalizedDistribution = Arrays.Normalize(distribution);

                int classValueIndex = Arrays.MaxIndex(normalizedDistribution);

                return new Id3(
                    classAttribute
                    , classAttributeIndex
                    , normalizedDistribution[normalizedDistribution.MaxIndex()]
                    , normalizedDistribution);
            }
            else
            {
                var splitData = new DataSetBuilder().SplitDataSet(trainingData, maxInfoGainAttributeIdx);

                for (int j = 0; j < maxInfoGainAttribute.Values.Length; j++)
                {

                }
            }

            return null;
        }
    }

    public class Id3 : DistributionClassifier
    {
        /// <summary>
        /// Attribute used for splitting.
        /// </summary>
        public Core.Attribute SplitAttribute {get; private set;}

        /// <summary>
        /// Class distribution if node is leaf.
        /// </summary>
        public double[] Distribution { get; private set; }

        /// <summary>
        /// The node's successors.
        /// </summary>
        public IList<Id3> Successors { get; private set; }

        /// <summary>
        /// Class value if node is leaf.
        /// </summary>
        public double ClassValue {get; private set;}

        public Id3(Core.Attribute classAttribute
            , int classAttributeIndex
            , double classValue
            , double[] distribution)
            : base(classAttribute, classAttributeIndex)
        {
            ClassValue = classValue;
            Distribution = distribution;
        }

        public Id3(Core.Attribute classAttribute
            , int classAttributeIndex
            , double classValue
            , IList<Id3> successors)
            : base(classAttribute, classAttributeIndex)
        {
            Successors = new List<Id3>(successors);
        }

        public override double ClassifyInstance(Instance instance)
        {
            if (SplitAttribute == null)
            {
                return ClassValue;
            }

            return Successors[(int)instance[ClassAttributeIndex]].ClassifyInstance(instance);
        }

        public override double[] DistributionForInstance(Instance instance)
        {
            if (SplitAttribute == null)
            {
                return Distribution;
            }

            return Successors[(int)instance[ClassAttributeIndex]].DistributionForInstance(instance);
        }
    }
}
