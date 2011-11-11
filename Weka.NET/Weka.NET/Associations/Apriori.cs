using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;

namespace Weka.NET.Associations
{
    public class ItemSet
    {
        public IList<int?> Items {set;get;}

        public int Counter { set; get; }

        public int? this[int index]
        {
            get { return Items[index]; }
        }

        public bool ContainedBy(Instance instance)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].HasValue)
                {
                    if (false == instance[i].HasValue)
                    {
                        return false;
                    }

                    if (Items[i] != instance[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    public class Apriori
    {
        public int Cycles { set; get; }

        public int MinSupport { set; get; }

        public void BuildAssociationRules(DataSet instances)
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
