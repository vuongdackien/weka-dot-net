using System;
using System.Linq;
using System.Collections.Generic;
using Weka.NET.Lang;
using Weka.NET.Utils;
using System.Diagnostics.Contracts;

namespace Weka.NET.Associations
{
    [Immutable]
    public class ItemSet : IEquatable<ItemSet>
    {
        public IList<double?> Items { get; private set; }

        /// <summary>
        /// Persent of transactions containing the items defined by this ItemSet object.
        /// </summary>
        public int Support { get; private set; }

        public int Count { get; private set; }

        public double? this[int index]
        {
            get { return Items[index]; }
        }

        public ItemSet(IEnumerable<double?> items, int support)
        {
            Contract.Ensures(items != null);

            Items = items.ToList().AsReadOnly();
            
            Support = support;

            Count = (from i in items where i.HasValue select i).Count();
        }

        public bool Equals(ItemSet other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Count == Count && Support == other.Support && Arrays.AreEquals(other.Items, Items);
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.GetType() != typeof(ItemSet))
            {
                return false;
            }

            return Equals(other as ItemSet);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Count * 397) ^ (Items != null ? Arrays.GetHashCodeForArray(Items) : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("ItemSet[Size: {0}, Items: {1}]", Count, Arrays.ArrayToString(Items));
        }

        public bool Intersects(ItemSet other)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].HasValue && other.Items[i].HasValue)
                {
                    return true;
                }
            }

            return false;
        }

        public IList<double?> MergeItems(ItemSet itemSet)
        {


            var newItems = new List<double?>();

            for (int i = 0; i < itemSet.Items.Count; i++)
            {
                if (Items[i] == null && itemSet[i] != null)
                {
                    newItems.Add(itemSet[i]);
                }

                if (Items[i] != null && itemSet[i] == null)
                {
                    newItems.Add(Items[i]);
                }

                if (Items[i] == null && itemSet[i] == null)
                {
                    newItems.Add(null);
                }

                if (Items[i] != null && itemSet[i] != null && false == Items[i].Value.Equals(itemSet[i].Value))
                {
                    throw new ArgumentException("can't merge");
                }
            }

            return newItems;
        }
    }
}
