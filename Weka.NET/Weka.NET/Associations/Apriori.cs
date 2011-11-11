using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;

namespace Weka.NET.Associations
{
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
