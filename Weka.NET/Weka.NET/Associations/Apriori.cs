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

        public IEnumerable<ItemSet> BuildAssociationRules(DataSet instances)
        {
            return null;
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
