namespace Weka.NET.Associations
{
    using Weka.NET.Utils;
    using Weka.NET.Lang;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using Weka.NET.Core;
    
    public interface IItemSetBuilder
    {
        IDictionary<ItemSet, int> BuildItemSets(IDataSet dataSet);
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

        public IDictionary<ItemSet, int> BuildItemSets(IDataSet dataSet)
        {
            int necSupport = (int)(MinSupport * dataSet.Count);

            Console.WriteLine("Building ItemStets with necSupport={0}", necSupport);

            var supportByItems = BuildAndPruneSingletons(dataSet, necSupport);

            var itemsBySize = new Dictionary<int, HashSet<ItemSet>>();

            itemsBySize[1] = new HashSet<ItemSet>();
            
            itemsBySize[1].AddAll(supportByItems.Keys.ToList());

            RecursivelyBuildItems(1, itemsBySize, supportByItems, dataSet, necSupport);

            return supportByItems;
        }

        public void RecursivelyBuildItems(int actualSize, Dictionary<int, HashSet<ItemSet>> itemsBySize, IDictionary<ItemSet, int> supportByItems, IDataSet dataSet, int necSupport)
        {
            var newItemSets = new HashSet<ItemSet>();

            int intersectSize = actualSize - 1;

            foreach (var left in itemsBySize[actualSize])
            {
                foreach (var right in itemsBySize[actualSize])
                {
                    if (left == right) continue;

                    if (left.IsIntersecteable(right) && left.IntersectsCount(right) == intersectSize)
                    {
                        var newItems = left.Union(right);

                        int newItemsSupport = CalculateSupport(newItems, dataSet);

                        if (newItemsSupport < necSupport)
                        {
                            continue;
                        }

                        newItemSets.Add(newItems);

                        supportByItems[newItems] = newItemsSupport;
                    }
                }
            }

            if (newItemSets.Count == 0)
            {
                return;
            }
                
            itemsBySize[actualSize+1] = newItemSets;

            RecursivelyBuildItems(actualSize + 1, itemsBySize, supportByItems, dataSet, necSupport);
        }

        public int CalculateSupport(ItemSet items, IDataSet dataSet)
        {
            int count = 0;

            foreach (var instance in dataSet.Instances)
            {
                if (items.ContainedBy(instance))
                {
                    count++;
                }
            }

            return count;
        }

        public IDictionary<ItemSet, int> BuildAndPruneSingletons(IDataSet dataSet, int necSupport)
        {
            var singletons = BuildSingletonItems(dataSet);

            var supportByItems = new Dictionary<ItemSet, int>();

            foreach (var singleton in singletons)
            {
                int support = CalculateSupport(singleton, dataSet);

                if (support < necSupport)
                {
                    continue;
                }

                supportByItems[singleton] = support;
            }

            return supportByItems;
        }

        public HashSet<ItemSet> BuildSingletonItems(IDataSet dataSet)
        {
            var singletons = new HashSet<ItemSet>();

            for (int attributeIndex = 0; attributeIndex < dataSet.Attributes.Count; attributeIndex++)
            {
                var attributeValues = (dataSet.Attributes[attributeIndex] as NominalAttribute).Values;

                for (int valueIndex = 0; valueIndex < attributeValues.Length; valueIndex++)
                {
                    var values = new double?[dataSet.Attributes.Count];

                    values[attributeIndex] = valueIndex;

                    var items = new ItemSet(values);

                    singletons.Add(items);
                }
            }
            return singletons;
        }
    }
}
