namespace Weka.NET.Associations
{
    using System.Collections.Generic;
    using System;
    
    public interface IRuleBuilder
    {
        double MinConfidence { get; }

        IList<AssociationRule> BuildRules(IDictionary<ItemSet, int> supportByItems);
    }

    public class RuleBuilder : IRuleBuilder
    {
        public double MinConfidence { get; private set; }

        public RuleBuilder(double minConfidence)
        {
            MinConfidence = minConfidence;
        }

        public IList<AssociationRule> BuildRules(IDictionary<ItemSet, int> supportByItems)
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

                    var candidate = BuildRule(left, right, supportByItems);

                    if (candidate.Confidence >= MinConfidence)
                    {
                        rules.Add(candidate);
                    }
                }
            }

            return rules;
        }

        public static double CalculateConfidence(int fullRuleSupport, int premisseSupport)
        {
            return (double)fullRuleSupport / (double)premisseSupport;
        }

        public AssociationRule BuildRule(ItemSet left, ItemSet right, IDictionary<ItemSet, int> supportByItems)
        {
            int necFullSupport = supportByItems[left.Union(right)];

            double confidence = CalculateConfidence(necFullSupport, supportByItems[left]);

            return new AssociationRule(premisse: left, consequence: right, confidence: confidence);
        }
    }
}
