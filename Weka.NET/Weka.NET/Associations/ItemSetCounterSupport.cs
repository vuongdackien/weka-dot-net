using System.Collections.Generic;
using System.Linq;
using Weka.NET.Core;
using Weka.NET.Lang;

namespace Weka.NET.Associations
{
    [NonThreadSafe]
    public class ItemSetCounterSupport
    {
        readonly DataSet dataSet;

        readonly IDictionary<ItemSet, int> supports;

        public ItemSetCounterSupport(DataSet dataSet)
        {
            this.dataSet = dataSet;
            this.supports = new Dictionary<ItemSet, int>();
        }

        public int CountSupportFor(ItemSet itemSet)
        {
            if (supports.ContainsKey(itemSet))
            {
                return supports[itemSet];
            }

            /*
             * dataSet.Instances.Count(itemSet.ContainedBy);
             * 
             * is the Linq / group expression for:
             * 
             * foreach (var instance in dataSet.Instances)
             * {
             *      if (itemSet.ContainedBy(instance))
             *      {
             *          count++;
             *      }
             * }
             */

            int count = dataSet.Instances.Count(itemSet.ContainedBy);

            supports[itemSet] = count;

            return count;
        }
    }
}
