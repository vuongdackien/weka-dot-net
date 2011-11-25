using System.Collections.Generic;
using System.Linq;
using Weka.NET.Core;
using Weka.NET.Lang;

namespace Weka.NET.Associations
{
    [NonThreadSafe]
    public class ItemSetSupports
    {
        readonly DataSet dataSet;

        readonly IDictionary<ItemSet, int> supports;

        public ItemSetSupports(DataSet dataSet)
        {
            this.dataSet = dataSet;
            this.supports = new Dictionary<ItemSet, int>();
        }

        public int this[ItemSet index]
        {
            get { return supports[index]; }
        }

        public void AddItemSets(IEnumerable<ItemSet> itemSets)
        {
            foreach (var itemSet in itemSets)
            {
                AddItemSets(itemSet);
            }
        }

        public void AddItemSets(ItemSet itemSet)
        {
            if (supports.ContainsKey(itemSet))
            {
                return;
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
        }
    }
}
