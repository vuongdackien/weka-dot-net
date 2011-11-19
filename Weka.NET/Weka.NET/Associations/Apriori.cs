using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;
using System.Diagnostics.Contracts;
using Weka.NET.Lang;

namespace Weka.NET.Associations
{
    [Serializable]
    public class Apriori
    {
        const int DefaultMaxRulesCount = 10;

        public int MaxRulesCount { set; get; }

        /// <summary>
        /// Significance level for optional significance test.
        /// </summary>
        public double SignificanceLevel { get { return -1d; } }

        /// <summary>
        /// The minimum support
        /// </summary>
        const double DefaultMinSupport = 1d;
        
        public double MinSupport { private set; get; }

        /** The minimum confidence. */
        const double DefaultMinConfidence = 0.9d;

        public double MinConfidence;

        /// <summary>
        /// Lower bound min support
        /// </summary>
        const double DefaultLowerBoundMinSupport = .1;

        public double LowerBoundMinSupport { private set; get; }

        /** Delta by which m_minSupport is decreased in each iteration. */
        const double DefaultDelta = .05;

        public double Delta { set; get; }

        public List<AssociationRule> AllRules { get; private set; }

        /// <summary>
        /// ItemSet counts
        /// </summary>
        readonly IDictionary<ItemSet, int> itemSetCounts;

        public IDictionary<ItemSet,int> ItemSetCounts 
        {
            get { return new Dictionary<ItemSet, int>(itemSetCounts); }
        }
        
        public int Cycles { set; get; }

        /** The set of all sets of itemsets L. */
        readonly IList<IList<ItemSet>> allItemSets = new List<IList<ItemSet>>();

        public Apriori(double minSupport)
        {
            MaxRulesCount = DefaultMaxRulesCount;
            AllRules = new List<AssociationRule>();
            MinSupport = minSupport;
            itemSetCounts = new Dictionary<ItemSet, int>();
            LowerBoundMinSupport = DefaultLowerBoundMinSupport;
            Delta = DefaultDelta;
        }


        public IList<AssociationRule> PruneRules(IList<AssociationRule> rulesToPrune, double minConfidence)
        {
            var prunedRules = from r in rulesToPrune where r.CalculateConfidence() >= minConfidence select r;

            return prunedRules.ToList();
        }

        public IList<ItemSet> BuildSingletons(IList<Weka.NET.Core.Attribute> attributes)
        {
            CodeContract.NotSupportedNumericAttributes(attributes);

            var singletons = new List<ItemSet>();

            for (int attributeIndex = 0; attributeIndex < attributes.Count; attributeIndex++)
            {
                var values = (attributes[attributeIndex] as NominalAttribute).Values;

                for (int valueIndex = 0; valueIndex < values.Length; valueIndex++)
                {
                    var items = new int?[attributes.Count];

                    items[attributeIndex] = valueIndex;

                    var itemSet = new ItemSet(items: items);

                    singletons.Add(itemSet);
                }
            }

            return singletons;
        }

        public void FindLargeItemSets(DataSet dataSet)
        {
            var necSupport = (double)(MinSupport * (double)dataSet.Count);
            
            var kSets = BuildSingletons(dataSet.Attributes);
            
            UpdateCounts(dataSet, kSets.ToArray());

            kSets = DeleteItemSets(kSets, (int) necSupport);

            if (kSets.Count == 0)
            {
                return;
            }

            int size = 0;

            do
            {
                allItemSets.Add(kSets);

                var kMinusOneSets = new List<ItemSet>(kSets);
                
                kSets = MergeAllItemSets(kMinusOneSets, size);

                UpdateCounts(dataSet, kMinusOneSets);

                kSets = PruneItemSets(kSets, kMinusOneSets);

                UpdateCounts(dataSet, kSets);

                kSets = DeleteItemSets(kSets, (int) necSupport);
                
                size++;

            } while (kSets.Count > 0);

        }

        public IEnumerable<AssociationRule> BuildAssociationRules(DataSet dataSet)
        {
            if (dataSet.ContainsStringAttribute())
            {
                throw new ArgumentException("Can't handle string attributes!");
            }

            int cycles = 0;

            do
            {
                FindLargeItemSets(dataSet);

                if (SignificanceLevel != -1)
                {
                }
                else
                {
                    FindRulesQuickly();
                }

                SortRulesAccordingToTheirSupport();

                SortRulesAccordingToTheirConfidence();

                MinSupport -= Delta;

                cycles++;
   
            } while (AllRules.Count < MaxRulesCount && MinSupport.GreaterOrEqualsTo(LowerBoundMinSupport));

            return null;
        }

        private void SortRulesAccordingToTheirConfidence()
        {
        }

        private void SortRulesAccordingToTheirSupport()
        {

        }

        public IList<ItemSet> PruneItemSets(IList<ItemSet> toPrune, IList<ItemSet> kMinusOne)
        {
            var pruned = new List<ItemSet>();

            foreach (var itemSet in toPrune)
            {
                var prunedValues = itemSet.Items.ToArray();

                int j;

                for (j = 0; j < prunedValues.Length; j++)
                {
                    if (false == prunedValues[j].HasValue)
                    {
                        continue;
                    }

                    var help = prunedValues[j];

                    prunedValues[j] = null;

                    if (false == kMinusOne.Contains(new ItemSet(prunedValues)))
                    {
                        prunedValues[j] = help;

                        break;
                    }

                    prunedValues[j] = help;
                }

                if (j == prunedValues.Length)
                {
                    pruned.Add(new ItemSet(items: prunedValues));
                }
            }
            return pruned;
        }

        public IList<ItemSet> MergeAllItemSets(IList<ItemSet> itemSets, int size)
        {
            var newItemSets = new List<ItemSet>();

            for(int firstIndex = 0;firstIndex<itemSets.Count;firstIndex++)
            {
                for (int secondIndex = firstIndex + 1; secondIndex < itemSets.Count; secondIndex++)
                {
                    MergeItems(itemSets, firstIndex, secondIndex, newItemSets, size);
                }
            }

            return newItemSets;
        }

        protected void MergeItems(IList<ItemSet> itemSets, int firstIndex, int secondIndex, IList<ItemSet> newItemSets, int size)
        {
            var newValues = new int?[itemSets[firstIndex].Items.Count];

            //Find and copy common prefix of size 'size'
            int numFound = 0;

            int k = 0;

            while (numFound < size) //up to size
            {
                if (itemSets[firstIndex].Items[k] != itemSets[secondIndex].Items[k])
                {
                    return;
                }

                if (itemSets[firstIndex].Items[k].HasValue)
                {
                    numFound++;
                }

                newValues[k] = itemSets[firstIndex].Items[k];

                k++;
            }

            while (k < itemSets[firstIndex].Items.Count)
            {
                if (itemSets[firstIndex].Items[k].HasValue && itemSets[secondIndex].Items[k].HasValue)
                {
                    break;
                }

                newValues[k] = itemSets[firstIndex].Items[k].HasValue ?
                    itemSets[firstIndex].Items[k] : itemSets[secondIndex].Items[k];

                k++;
            }

            // Check difference
            if (k == itemSets[firstIndex].Items.Count)
            {
                newItemSets.Add(new ItemSet(items: newValues));
            }

        }

        public IList<ItemSet> DeleteItemSets(IList<ItemSet> itemSets, int minSupport)
        {
            var newItemSets = (from itemSet in ItemSetCounts where itemSet.Value >= minSupport select itemSet.Key).ToList();

            return newItemSets;
        }

        public IList<int> UpdateCounts(DataSet dataSet, IEnumerable<ItemSet> itemSets)
        {
            return UpdateCounts(dataSet, itemSets.ToArray());
        }

        public IList<int> UpdateCounts(DataSet dataSet, params ItemSet[] itemSets)
        {
            var counts = new List<int>();

            foreach (var itemSet in itemSets)
            {
                if (itemSetCounts.ContainsKey(itemSet))
                {
                    continue; //already count
                }

                foreach (var instance in dataSet.Instances)
                {
                    if (itemSet.ContainedBy(instance))
                    {
                        if (itemSetCounts.ContainsKey(itemSet))
                        {
                            itemSetCounts[itemSet]++;
                        }
                        else
                        {
                            itemSetCounts[itemSet] = 1;
                        }
                    }
                }

                counts.Add(itemSetCounts[itemSet]);
            }

            return counts.AsReadOnly();
        }


        protected void FindRulesBruteForce()
        {

        }

        public IList<AssociationRule> GenerateAllRulesWithOneItemInTheConsequence(ItemSet itemSet, DataSet dataSet)
        {
            var rules = new List<AssociationRule>();

            for (int i = 0; i < itemSet.Items.Count; i++)
            {
                if (false == itemSet.Items[i].HasValue)
                {
                    continue;
                }

                //Build and get count for premise
                var premiseValues = itemSet.Items.ToArray();

                premiseValues[i] = null;

                var premise = new ItemSet(premiseValues);

                int premiseCount = UpdateCounts(dataSet, premise)[0];

                //Build and get count for consequence
                var consequenceValues = new int?[itemSet.Items.Count];

                consequenceValues[i] = itemSet.Items[i];

                var consequence = new ItemSet(consequenceValues);

                int consequenceCount = UpdateCounts(dataSet, consequence)[0];

                //finally add rule
                rules.Add(new AssociationRule(
                    premise:premise
                    , premiseCount:premiseCount
                    , consequence:consequence
                    , consequenceCount:consequenceCount));
            }

            return rules;
        }




        public IList<AssociationRule> GenerateRules(double minConfidence, IList<ItemSet> itemSets, int itemSetSize)
        {
            var premises = new List<int?>();

            var consequences = new List<int?>();

            // Generate all rules with one item in the consequence.


            return null;
        }

        protected void FindRulesQuickly()
        {
            for (int j=0;j<allItemSets.Count;j++)
            {
                foreach (IList<ItemSet> itemSets in allItemSets[j])
                {
                    var rules = GenerateRules(MinConfidence, itemSets, j + 1);

                    AllRules.AddRange(rules);
                }
            }

        }
    }



}
