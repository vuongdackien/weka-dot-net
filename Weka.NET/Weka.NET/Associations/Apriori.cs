using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;

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

        public IEnumerable<ItemSet> BuildAssociationRules(DataSet dataSet)
        {
            return null;
        }

        public IList<ItemSet> BuildSingletons(IList<Weka.NET.Core.Attribute> attributes)
        {
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

            int i = 0;

            do
            {
                m_Ls.Add(kSets);

                var kMinusOneSets = new List<ItemSet>(kSets);
                
                kSets = MergeAllItemSets(kMinusOneSets, i);

                var itemSetsWithCounter = GetHashtable(kMinusOneSets, kMinusOneSets.Count);

                itemsWithCounters.Add(itemSetsWithCounter);

                kSets = PruneItemSets(kSets, itemSetsWithCounter);

                UpdateCounters(kSets, dataSet);

                kSets = DeleteItemSets(kSets, necSupport);
                
                i++;

            } while (kSets.Count > 0);

        }

        private IList<ItemSet> PruneItemSets(IList<ItemSet> kSets, IDictionary<ItemSet, int> itemSetsWithCounter)
        {
            throw new NotImplementedException();
        }

        private IDictionary<ItemSet, int> GetHashtable(IList<ItemSet> kMinusOneSets, int initialSize)
        {
            throw new NotImplementedException();
        }

        private IList<ItemSet> MergeAllItemSets(IList<ItemSet> kMinusOneSets, int i)
        {
            throw new NotImplementedException();
        }

        private IList<ItemSet> DeleteItemSets(IList<ItemSet> kSets, double necSupport)
        {
            throw new NotImplementedException();
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
