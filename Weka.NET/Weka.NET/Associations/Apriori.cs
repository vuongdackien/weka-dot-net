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

        public Apriori(int minSupport)
        {
            MinSupport = minSupport;
        }

        public IEnumerable<ItemSet> BuildAssociationRules(DataSet dataSet)
        {
            var singletons = new List<ItemSet>();

            for (int attributeIndex=0;attributeIndex<dataSet.Attributes.Count;attributeIndex++)
            {
                var values = (dataSet.Attributes[attributeIndex] as NominalAttribute).Values;

                for(int valueIndex=0;valueIndex<values.Length;valueIndex++)
                {
                    var items = new int?[dataSet.Attributes.Count];

                    items[attributeIndex] = valueIndex;

                    var itemSet = new ItemSet(items: items);

                    singletons.Add(itemSet);
                }
            }

            return singletons;
        }

        protected void FindLargeItemSets()
        {
        }

        protected void FindRulesBruteForce()
        {

        }

        protected void FindRulesQuickly()
        {

        }
    }



}
