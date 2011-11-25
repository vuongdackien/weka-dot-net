using System.Collections.Generic;
using System.Linq;
using Weka.NET.Core;
using Weka.NET.Utils;
using System;

namespace Weka.NET.Associations
{
    public class ItemSets
    {
        public IDictionary<int, HashSet<ItemSet>> SetsBySize { private set; get; }

        public IDictionary<ItemSet, int> Counts { private set; get; }

        public ItemSets()
        {
            SetsBySize = new Dictionary<int, HashSet<ItemSet>>();
            Counts = new Dictionary<ItemSet, int>();
        }
    }

    public interface IItemSetBuilder
    {
        double MinSupport { get; }

        ItemSets BuildItemSets(DataSet dataSet);
    }

    public class ItemSetBuilder : IItemSetBuilder
    {
        public double MinSupport { get; private set; }

        public ItemSetBuilder(double minSupport)
        {
            MinSupport = minSupport;
        }

        public ItemSets BuildItemSets(DataSet dataSet)
        {
            double necSupport = MinSupport * (double)dataSet.Count;

            var stuffs = new ItemSetBuilderStuff(dataSet, (int) necSupport);

            var singletons = BuildSingletons(dataSet.Attributes);

            stuffs.AddAll(singletons);

            CheckCountMaxSizeIs(stuffs, 1);

            FindSets(size:1, itemSets:stuffs);
            
            return stuffs.ItemSets;
        }

        public void CheckCountMaxSizeIs(ItemSetBuilderStuff stuffs, int expectedMaxSize)
        {
            if (expectedMaxSize != stuffs.ItemSets.SetsBySize.Keys.Max())
            {
                throw new Exception("expecting: " + expectedMaxSize + " but was: " + stuffs.ItemSets.SetsBySize.Keys.Max());
            }
        }

        public void FindSets(int size, ItemSetBuilderStuff itemSets)
        {
            if (false == itemSets.ItemSets.SetsBySize.ContainsKey(size))
            {
                return;
            }

            var itemsToEnlarge = itemSets.ItemSets.SetsBySize[size].ToList();

            if (itemsToEnlarge.Count == 1)
            {
                return;
            }

            var allCombineds = new List<ItemSet>();

            for (int i = 0; i < itemsToEnlarge.Count; i++)
            {
                for (int j = i+1; j < itemsToEnlarge.Count; j++)
                {
                    var combineds = CombineItemSets(itemsToEnlarge, i, j, size);

                    allCombineds.AddRange(combineds);
                }
            }

            itemSets.AddAll(allCombineds);

            if (false == itemSets.ItemSets.SetsBySize.ContainsKey(size))
            {
                return;
            }

          //  CheckCountMaxSizeIs(itemSets, size);

            size++;

            FindSets(size, itemSets);
        }

        public IEnumerable<ItemSet> CombineItemSets(List<ItemSet> items, int i, int j, int size)
        {
            var combineds = new List<ItemSet>();

            for (int v1 = 0; v1 < items[i].Items.Count; v1++)
            {
                if (items[i].Items[v1].HasValue && false == items[j].Items[v1].HasValue)
                {
                    var newValues = new double?[items[i].Items.Count];

                    items[j].Items.CopyTo(newValues, 0);

                    newValues[v1] = items[i].Items[v1].Value;

                    combineds.Add(new ItemSet(newValues));

                    continue;
                }

                if (false == items[i].Items[v1].HasValue && items[j].Items[v1].HasValue)
                {
                    var newValues = new double?[items[i].Items.Count];

                    items[i].Items.CopyTo(newValues, 0);

                    newValues[v1] = items[j].Items[v1].Value;

                    combineds.Add(new ItemSet(newValues));

                    continue;
                }
            }

            return combineds;
        }

        public HashSet<ItemSet> BuildSingletons(IList<Weka.NET.Core.Attribute> attributes)
        {
            var singletons = new HashSet<ItemSet>(new ItemSetComparer());

            for (int attributeIndex = 0; attributeIndex < attributes.Count; attributeIndex++)
            {
                var values = (attributes[attributeIndex] as NominalAttribute).Values;

                for (int valueIndex = 0; valueIndex < values.Length; valueIndex++)
                {
                    var items = new double?[attributes.Count];

                    items[attributeIndex] = valueIndex;

                    var itemSet = new ItemSet(items: items);

                    singletons.Add(itemSet);
                }
            }

            return singletons;
        }

        public class ItemSetBuilderStuff
        {
            readonly DataSet dataSet;

            readonly int necSupport;

            public ItemSets ItemSets {get;private set;}

            public ItemSetBuilderStuff(DataSet dataSet, int necSupport)
            {
                this.dataSet = dataSet;
                this.necSupport = necSupport;
                this.ItemSets = new ItemSets();
            }

            public void Add(ItemSet itemSet)
            {
                if (ItemSets.Counts.ContainsKey(itemSet))
                {
                    return;
                }

                var count = CountSupport(itemSet);

                if (count < necSupport)
                {
                    return;
                }

                ItemSets.Counts[itemSet] = count;

                if (false == ItemSets.SetsBySize.ContainsKey(itemSet.Size))
                {
                    ItemSets.SetsBySize[itemSet.Size] = new HashSet<ItemSet>();
                }

                ItemSets.SetsBySize[itemSet.Size].Add(itemSet);
            }

            public void AddAll(IEnumerable<ItemSet> itemSets)
            {
                foreach (var itemSet in itemSets)
                {
                    Add(itemSet);
                }
            }

            internal int CountSupport(ItemSet itemSet)
            {
                int count = 0;

                foreach (var instance in dataSet.Instances)
                {
                    if (itemSet.ContainedBy(instance))
                    {
                        count++;
                    }
                }

                return count;
            }

        }
    }
}
