using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;
using Weka.NET.Utils;

namespace Weka.NET.Associations
{
    public interface IApriori
    {
        double MinSupport { get; }

        int MaxNumRules { get; }

        IList<AssociationRule> BuildAssociationRules(DataSet dataSet);
    }

    [Serializable]
    public class Apriori : IApriori
    {
        public double MinSupport { private set; get; }

        public int MaxNumRules { private set; get; }

        public Apriori(double minSupport, int maxNumRules)
        {
            MinSupport = minSupport;
            MaxNumRules = maxNumRules;
        }

        public IList<AssociationRule> BuildAssociationRules(DataSet dataSet)
        {
            return null;
        }
    }
}

/*            CodeContract.NotSupportedStringAttributes(dataSet.Attributes);

            ItemSets BuildItemSets(DataSet dataSet, double minSupport);

            var execution = new AprioriState();

            var singletons = BuildSingletons(dataSet.Attributes);

            

            /*var executionParams = new AprioriExecution(dataSet);

            executionParams.AddItemSets();

            executionParams.BuildItemSets(MinSupport);

            executionParams.BuildRules(MaxNumRules);

            return execution.Rules;
        }

 */
