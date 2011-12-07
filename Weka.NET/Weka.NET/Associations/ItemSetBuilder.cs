using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;
using Weka.NET.Utils;

namespace Weka.NET.Associations
{
    public interface IItemSetBuilder
    {
        IDictionary<int, IList<ItemSet>> BuildItemSets(IDataSet dataSet);
    }

    public class ItemSetBuilder : IItemSetBuilder
    {
        public double MinSupport {get; private set;}

        public ItemSetBuilder(double minSupport)
        {
            if (minSupport < 0 || minSupport > 1)
            {
                throw new ArgumentException("MinSupport ranges between 0 and 1");
            }

            MinSupport = minSupport;
        }

        public IList<ItemSet> BuildSingletons(IDataSet dataSet)
        {
            var singletons = new List<ItemSet>();

            for (int attributeIndex = 0; attributeIndex < dataSet.Attributes.Count; attributeIndex++)
            {
                var values = (dataSet.Attributes[attributeIndex] as NominalAttribute).Values;

                for (int valueIndex = 0; valueIndex < values.Length; valueIndex++)
                {
                    var items = new double?[dataSet.Attributes.Count];

                    items[attributeIndex] = valueIndex;

                    int support = CountSupportFor(dataSet, items);

                    var itemSet = new ItemSet(items: items, support: support);

                    singletons.Add(itemSet);
                }
            }

            return singletons;
        }

        public int CountSupportFor(IDataSet dataSet, double?[] items)
        {
            int support = 0;

            foreach (var instance in dataSet.Instances)
            {
                if (ContainedBy(instance, items))
                {
                    support++;
                }
            }

            return support;
        }

        internal bool ContainedBy(Instance instance, double?[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].HasValue)
                {
                    if (false == instance[i].HasValue)
                    {
                        return false;
                    }

                    if (items[i] != instance[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public IList<ItemSet> MergeRightAgainstLeft(IDataSet dataSet, ItemSet first, ItemSet second)
        {
            var newItemSets = new List<ItemSet>();

            for (int i = 0; i < first.Items.Count; i++)
            {
                if (first[i].HasValue)
                {
                    continue;
                }

                if (false == second[i].HasValue)
                {
                    continue;
                }

                double?[] newItems = new double?[first.Items.Count];

                first.Items.CopyTo(newItems, 0);

                newItems[i] = second[i];

                int support = CountSupportFor(dataSet, newItems);

                newItemSets.Add(new ItemSet(items: newItems, support:support));
            }

            return newItemSets;
        }

        public IList<ItemSet> CombineItemSets(IDataSet dataSet, IList<ItemSet> itemSets)
        {
            var newItemSets = new HashSet<ItemSet>();

            for (int i = 0; i < itemSets.Count; i++)
            {
                for (int j = 0; j < itemSets.Count; j++)
                {
                    var mergedItems = MergeRightAgainstLeft(dataSet, itemSets[i], itemSets[j]);

                    newItemSets.AddAll(mergedItems);
                }
            }

            return newItemSets.ToList();
        }

        public IDictionary<int, IList<ItemSet>> BuildItemSets(IDataSet dataSet)
        {
            //count == 1
            // x    == .2

            //.2 * count
            
            
            int necSupport = (int)(MinSupport * (double)dataSet.Count);

            var itemSetsByCount = new Dictionary<int, IList<ItemSet>>();

            int currentSize = 1;

            var singletons = BuildSingletons(dataSet);

            itemSetsByCount[currentSize] = Prune(singletons, necSupport);

            while (itemSetsByCount[currentSize].Count > 0)
            {
                var nextItemSets = CombineItemSets(dataSet, itemSetsByCount[currentSize]);

                var prunnedNextItemSets = Prune(nextItemSets, necSupport);

                currentSize++;

                itemSetsByCount[currentSize] = prunnedNextItemSets;
            }

            return itemSetsByCount;
        }

        public IList<ItemSet> Prune(IList<ItemSet> toPrune, int necSupport)
        {
            var prunned = toPrune.Where(item => item.Support > necSupport);

            return prunned.ToList();
        }
    }
}
