using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;

namespace Weka.NET.Classifiers
{
    public class OneRuleClassifier : IClassifier
    {
        const int DEFAULT_MIN_BUCKET_SIZE = 6;

        public OneRule ExtractedRule { get; private set; }

        public int MinBucketSize { get; private set; }

        public static OneRuleClassifier BuildClassifier(DataSet trainingSet, int classIndex)
        {
            if (trainingSet.CheckForStringAttributes())
            {
                throw new ArgumentException("");
            }

            if (trainingSet.CheckForNumericAttributes())
            {
                throw new ArgumentException("");
            }

            var dataSetBuilder = new DataSetTransformer(trainingSet);

            dataSetBuilder.DeleteWithMissingClass(classIndex);

            if (dataSetBuilder.Count == 0)
            {
                throw new Exception();
            }

            var cleansedTrainingSet = dataSetBuilder.Build();

            OneRule bestRuleSoFar = null;

            foreach(var attribute in cleansedTrainingSet.Attributes)
            {
                var candidateRule = BuildRule(attribute, cleansedTrainingSet);

                if (bestRuleSoFar == null || bestRuleSoFar.CorrectCount > candidateRule.CorrectCount)
                {
                    bestRuleSoFar = candidateRule;
                }
            }

            return new OneRuleClassifier(rule: bestRuleSoFar);
        }

        private static OneRule BuildRule(Core.Attribute attribute, DataSet cleansedTrainingSet)
        {
            throw new NotImplementedException();
        }

        public OneRuleClassifier(OneRule rule)
            : this(rule: rule, minBucketSize: DEFAULT_MIN_BUCKET_SIZE)
        {
        }

        public OneRuleClassifier(OneRule rule, int minBucketSize)
        {
            ExtractedRule = rule;
            MinBucketSize = minBucketSize;
        }

        public double ClassifyInstance(Instance instance)
        {
            if (false == instance[ExtractedRule.ClassAttribute.Index].HasValue)
            {
                if (ExtractedRule.MissingValueClass.HasValue)
                {
                    return ExtractedRule.MissingValueClass.Value;
                }

                return 0;
            }

            if (ExtractedRule.ClassAttribute is NominalAttribute)
            {
                return instance[ExtractedRule.ClassAttribute.Index].Value;
            }

            var predicted = 0;

            while (predicted < ExtractedRule.Breakpoints.Length
                 && instance[ExtractedRule.ClassAttribute.Index].Value >= ExtractedRule.Breakpoints[predicted])
            {
                predicted++;
            }
            return ExtractedRule.Classifications[predicted];
        }
    }

    public class OneRule
    {
        public Weka.NET.Core.Attribute ClassAttribute { get; private set; }

        public int InstancesCount { get; private set; }

        public double? MissingValueClass { set; get; }

        public Weka.NET.Core.Attribute TestAttribute { get; private set; }

        public int CorrectCount { get; private set; }

        public double[] Breakpoints { get; private set; }

        public double[] Classifications { get; private set; }

    }
}
