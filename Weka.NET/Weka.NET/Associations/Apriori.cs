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
        /// The minimum support
        /// </summary>
        public int MinSupport { private set; get; }
        
        public int Cycles { set; get; }

        /** The set of all sets of itemsets L. */
        IList<IList<ItemSet>> m_Ls;

        IList<IDictionary<ItemSet, int>> itemsWithCounters;

        public Apriori(int minSupport)
        {
            MinSupport = minSupport;
        }

        public IList<ItemSet> BuildSingletons(IList<Weka.NET.Core.Attribute> attributes)
        {
            Contract.Ensures(attributes.Any(attribute => attribute is NumericAttribute), "Can't handle numeric attributes");

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
            
            UpdateCounters(kSets, dataSet);

            DeleteItemSets(kSets, necSupport);

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

                var itemSetsWithCounter = GetHashtable(kMinusOneSets, kMinusOneSets.Count);

                itemsWithCounters.Add(itemSetsWithCounter);

                kSets = PruneItemSets(kSets, itemSetsWithCounter);

                UpdateCounters(kSets, dataSet);

                kSets = DeleteItemSets(kSets, necSupport);
                
                size++;

            } while (kSets.Count > 0);

        }

        public IEnumerable<ItemSet> BuildAssociationRules(DataSet dataSet)
        {
            return null;
        }

        private IList<ItemSet> PruneItemSets(IList<ItemSet> toPrune, IDictionary<ItemSet, int> kMinusOne)
        {
            var pruned = new List<ItemSet>();

            foreach (var itemSet in toPrune)
            {
                var prunedValues = new List<int?>(itemSet.Items);

                for (int j = 0; j < prunedValues.Count; j++)
                {
                    if (false == prunedValues[j].HasValue)
                    {
                        continue;
                    }

                    var help = prunedValues[j];

                    prunedValues[j] = null;

                    prunedValues[j] = help;

                    if (false == kMinusOne.ContainsKey(itemSet))
                    {
                        break;
                    }

                    if (j == itemSet.Items.Count)
                    {
                        pruned.Add(new ItemSet(items: prunedValues));
                    }
                }
            }
            return pruned;
        }

        public IDictionary<ItemSet, int> GetHashtable(IList<ItemSet> itemSets, int initialSize)
        {
            var itemSetTable = new Dictionary<ItemSet, int>();

            foreach (ItemSet itemSet in itemSets)
            {
                itemSetTable[itemSet] = itemSet.Counter;
            }

            return itemSetTable;
        }

        public IList<ItemSet> MergeAllItemSets(IList<ItemSet> itemSets, int size)
        {
            var newItemSets = new List<ItemSet>();

            for (int i = 0; i < itemSets.Count;i++ )
            {
                var first = itemSets[i];

                exit: for (int j = i + 1; j < itemSets.Count; j++)
                {
                    var second = itemSets[j];

                    var newValues = new int?[first.Items.Count];

                    // Find and copy common prefix of size 'size'
                    int numFound = 0;
                    int k = 0;
                    while (numFound < size)
                    {
                        if (first.Items[k] == second.Items[k])
                        {
                            if (first.Items[k].HasValue)
                            {
                                numFound++;
                            }

                            newValues[k] = first.Items[k];
                        }
                        else
                        {
                            goto exit; //will fix this
                        }

                        k++;
                    }

                    // Check difference
                    while (k < first.Items.Count)
                    {
                        if ((first.Items[k].HasValue) && (second.Items[k].HasValue))
                        {
                            break;
                        }   
                        else
                        {
                            if (first.Items[k].HasValue)
                            {
                                newValues[k] = first.Items[k];
                            }
                            else
                            {
                                newValues[k] = second.Items[k];
                            }
                        }
                        k++;
                    }

                    if (k == first.Items.Count)
                    {
                        var newItemSet = new ItemSet(counter: 0, items:newValues);

                        newItemSets.Add(newItemSet);
                    }
                }
            }

            return newItemSets;
        }

        private IList<ItemSet> DeleteItemSets(IList<ItemSet> itemSets, double minSupport)
        {
            var newItemSets = (from itemSet in itemSets where itemSet.Counter >= minSupport select itemSet).ToList();

            return newItemSets;
        }

        public void UpdateCounters(IEnumerable<ItemSet> itemSets, DataSet dataSet)
        {
            foreach (var instance in dataSet.Instances)
            {
                foreach (var itemSet in itemSets)
                {
                    if (itemSet.ContainedBy(instance))
                    {
                        itemSet.UpdateCounter();
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
