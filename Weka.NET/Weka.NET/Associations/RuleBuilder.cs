using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Utils;

namespace Weka.NET.Associations
{
    public interface IRuleBuilder
    {
        double MinConfidence { get; }

        IList<AssociationRule> BuildRules(IDictionary<int, IList<ItemSet>> itemSets);
    }

    public class RuleBuilder : IRuleBuilder
    {
        public double MinConfidence { get; private set; }

        public RuleBuilder(double minConfidence)
        {
            MinConfidence = minConfidence;
        }

        public IList<AssociationRule> BuildRules(IDictionary<int, IList<ItemSet>> itemSets)
        {
            var rules = new HashSet<AssociationRule>();

            if (itemSets.Count < 2)
            {
                return rules.ToList().AsReadOnly();
            }

            for (int size = 2; size <= itemSets.Count; size++)
            {
                foreach (var candidateRule in itemSets[size])
                {
                    var candidates = BuildRules(candidateRule, itemSets[size - 1]);

                    rules.AddAll(candidates);
                }
            }

            var prunned = rules.Where(r => r.Confidence > MinConfidence);

            return prunned.ToList().AsReadOnly();
        }

        public IList<AssociationRule> BuildRules(ItemSet fullRule, IList<ItemSet> itemSets)
        {
            var rules = new List<AssociationRule>();

            for (int i = 0; i < itemSets.Count; i++)
            {
                for (int j = 0; j < itemSets.Count; j++)
                {
                    if (false == itemSets[i].Intersects(itemSets[j]))
                    {
                        var mergeItems = itemSets[i].MergeItems(itemSets[j]);

                        if (Arrays.AreEquals(fullRule.Items, mergeItems))
                        {
                            rules.Add(new AssociationRule(premisse: itemSets[i], consequence: itemSets[j], fullRule: fullRule));

                            rules.Add(new AssociationRule(premisse: itemSets[j], consequence: itemSets[i], fullRule: fullRule));

                            return rules;
                        }
                    }

                }
            }

            return rules;
        }
    }
}
