using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;
using System.Diagnostics.Contracts;


namespace Weka.NET.Associations
{
    [Serializable]
    public class Apriori
    {
        /// <summary>
        /// ItemSet counts
        /// </summary>
        readonly IDictionary<ItemSet, int> itemSetCounts;

        public IDictionary<ItemSet,int> ItemSetCounts 
        {
            get { return new Dictionary<ItemSet, int>(itemSetCounts); }
        }

        /// <summary>
        /// The minimum support
        /// </summary>
        public int MinSupport { private set; get; }
        
        public int Cycles { set; get; }

        /** The set of all sets of itemsets L. */
        IList<IList<ItemSet>> m_Ls;

        public Apriori(int minSupport)
        {
            MinSupport = minSupport;
            itemSetCounts = new Dictionary<ItemSet, int>();
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
            
            UpdateCounts(kSets, dataSet);

            kSets = DeleteItemSets(kSets, (int) necSupport);

            if (kSets.Count == 0)
            {
                return;
            }

            int size = 0;

            do
            {
                m_Ls.Add(kSets);

                var kMinusOneSets = new List<ItemSet>(kSets);
                
                kSets = MergeAllItemSets(kMinusOneSets, size);

                UpdateCounts(kMinusOneSets, dataSet);

                kSets = PruneItemSets(kSets, kMinusOneSets);

                UpdateCounts(kSets, dataSet);

                kSets = DeleteItemSets(kSets, (int) necSupport);
                
                size++;

            } while (kSets.Count > 0);

        }

        public IEnumerable<ItemSet> BuildAssociationRules(DataSet dataSet)
        {
            return null;
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

        public void UpdateCounts(IEnumerable<ItemSet> itemSets, DataSet dataSet)
        {
            foreach (var itemSet in itemSets)
            {
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
            }
        }


        protected void FindRulesBruteForce()
        {

        }

        protected void FindRulesQuickly()
        {

        }
    }



}
