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
        public double? SignificanceLevel { set; get; }

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
        public double MinSupport { private set; get; }
        
        public int Cycles { set; get; }

        /** The set of all sets of itemsets L. */
        readonly IList<IList<ItemSet>> itemSets = new List<IList<ItemSet>>();

        public Apriori(double minSupport)
        {
            MinSupport = minSupport;
            itemSetCounts = new Dictionary<ItemSet, int>();
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
                itemSets.Add(kSets);

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
            FindLargeItemSets(dataSet);

            if (SignificanceLevel.HasValue)
            {
                FindRulesQuickly();
            }
            else
            {

            }

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

        public IList<int> UpdateCounts(DataSet dataSet, IEnumerable<ItemSet> itemSets)
        {
            return UpdateCounts(dataSet, itemSets.ToArray());
        }

        public IList<int> UpdateCounts(DataSet dataSet, params ItemSet[] itemSets)
        {
            var counts = new List<int>();

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




        public IList<object> GenerateRules(double minConfidence, IList<ItemSet> itemSets, int itemSetSize)
        {
            var premises = new List<int?>();

            var consequences = new List<int?>();

            // Generate all rules with one item in the consequence.


            return null;
        }

        /*
 	public final FastVector[] generateRules(double minConfidence,
			FastVector hashtables, int numItemsInSet) {

		FastVector premises = new FastVector(), consequences = new FastVector(), conf = new FastVector();
		FastVector[] rules = new FastVector[3], moreResults;
		ItemSet premise, consequence;
		Hashtable hashtable = (Hashtable) hashtables
				.elementAt(numItemsInSet - 2);

		// Generate all rules with one item in the consequence.
		for (int i = 0; i < m_items.length; i++)
			if (m_items[i] != -1) {
				premise = new ItemSet();
				consequence = new ItemSet();
				premise.m_items = new int[m_items.length];
				consequence.m_items = new int[m_items.length];
				consequence.m_counter = m_counter;
				for (int j = 0; j < m_items.length; j++)
					consequence.m_items[j] = -1;
				System.arraycopy(m_items, 0, premise.m_items, 0, m_items.length);
				premise.m_items[i] = -1;
				consequence.m_items[i] = m_items[i];
				premise.m_counter = ((Integer) hashtable.get(premise))
						.intValue();
				premises.addElement(premise);
				consequences.addElement(consequence);
				conf.addElement(new Double(confidenceForRule(premise,
						consequence)));
			}
		rules[0] = premises;
		rules[1] = consequences;
		rules[2] = conf;
		pruneRules(rules, minConfidence);

		// Generate all the other rules
		moreResults = moreComplexRules(rules, numItemsInSet, 1, minConfidence,
				hashtables);
		if (moreResults != null)
			for (int i = 0; i < moreResults[0].size(); i++) {
				rules[0].addElement(moreResults[0].elementAt(i));
				rules[1].addElement(moreResults[1].elementAt(i));
				rules[2].addElement(moreResults[2].elementAt(i));
			}
		return rules;
	}
         */

        protected void FindRulesQuickly()
        {
            foreach (var itemSet in itemSets)
            {

            }

            /*
             FastVector[] rules;

            // Build rules
            for (int j = 1; j < m_Ls.size(); j++)
            {
                FastVector currentItemSets = (FastVector)m_Ls.elementAt(j);
                Enumeration eItemSets = currentItemSets.elements();
                while (eItemSets.hasMoreElements())
                {
                    ItemSet currentItemSet = (ItemSet)eItemSets.nextElement();
                    rules = currentItemSet.generateRules(m_minConfidence,
                            m_hashtables, j + 1);
                    for (int k = 0; k < rules[0].size(); k++)
                    {
                        m_allTheRules[0].addElement(rules[0].elementAt(k));
                        m_allTheRules[1].addElement(rules[1].elementAt(k));
                        m_allTheRules[2].addElement(rules[2].elementAt(k));
                    }
                }
            }
            */
        }
    }



}
