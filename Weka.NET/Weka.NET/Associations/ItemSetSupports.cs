using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;

namespace Weka.NET.Associations
{
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

            int count = 0;

            foreach (var instance in dataSet.Instances)
            {
                if (itemSet.ContainedBy(instance))
                {
                    count++;
                }
            }

            supports[itemSet] = count;
        }
    }
}
