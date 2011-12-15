namespace Weka.NET.Associations
{
    using System.Collections.Generic;
    using System;
    using Weka.NET.Core;
    
    public interface IRuleBuilder
    {
        double MinConfidence { get; }

        IList<AssociationRule> BuildRules(IDataSet dataSet, IDictionary<ItemSet, int> supportByItems);
    }

    public class RuleBuilder : IRuleBuilder
    {
        public double MinConfidence { get; private set; }

        public RuleBuilder(double minConfidence)
        {
            MinConfidence = minConfidence;
        }

        public IList<AssociationRule> BuildRules(IDataSet dataSet, IDictionary<ItemSet, int> supportByItems)
        {
            var rules = new List<AssociationRule>();

            foreach (var left in supportByItems.Keys)
            {
                foreach (var right in supportByItems.Keys)
                {
                    if (left == right) continue;

                    if (false == left.IsIntersecteable(right)) continue;

                    if (false == supportByItems.ContainsKey(left.Union(right))) continue;

                    if (left.IntersectsCount(right) > 0) continue;

                    var candidate = BuildRule(left, right, dataSet, supportByItems);

                    if (candidate.Confidence >= MinConfidence)
                    {
                        rules.Add(candidate);
                    }
                }
            }

            return rules;
        }

        public AssociationRule BuildRule(ItemSet left, ItemSet right, IDataSet dataSet, IDictionary<ItemSet, int> supportByItems)
        {
            int necFullSupport = supportByItems[left.Union(right)];

            double confidence = CalculateConfidence(left.Union(right), left, supportByItems);

            double ruleSupport = CalculateSupportProbability(left.Union(right), dataSet, supportByItems);

            double lift = CalculateLift(left, right, dataSet, supportByItems);

            return new AssociationRule(premisse: left, consequence: right, confidence: confidence, support: ruleSupport, lift: lift);
        }

        public double CalculateSupportProbability(ItemSet fullRule, IDataSet dataSet, IDictionary<ItemSet, int> supportByItems)
        {
            var fullRuleSupport = (double)supportByItems[fullRule];

            return fullRuleSupport / dataSet.Count;
        }

        public double CalculateConfidence(ItemSet fullRule, ItemSet premisse, IDictionary<ItemSet, int> supportByItems)
        {
            var fullRuleSupport = (double) supportByItems[fullRule];

            var premisseSupport = (double)supportByItems[premisse];

            return fullRuleSupport / (double)premisseSupport;
        }

        public double CalculateLift(ItemSet premisse, ItemSet consequence, IDataSet dataSet, IDictionary<ItemSet, int> supportByItems)
        {
            var premisseProbability = CalculateSupportProbability(premisse, dataSet, supportByItems);

            var consequenceProbability = CalculateSupportProbability(consequence, dataSet, supportByItems);

            return 1 / Math.Max(premisseProbability, consequenceProbability);
        }

    }
}
