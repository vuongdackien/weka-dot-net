using System;
using System.Collections.Generic;
using System.Linq;
using Weka.NET.Core;
using Weka.NET.Utils;

namespace Weka.NET.Associations
{
    public class ItemSetComparer : IEqualityComparer<ItemSet>
    {
        public bool Equals(ItemSet left, ItemSet right)
        {
            return left.Equals(right);
        }

        public int GetHashCode(ItemSet itemSet)
        {
            return itemSet.GetHashCode();
        }
    }

    /// <summary>
    /// Fill description...
    /// <remarks="This is the C# port for ItemSet.java originally developed by Eibe Frank"/>
    /// </summary>
    public class ItemSet : IEquatable<ItemSet>
    {
        public int Size { get; private set; }

        public IList<double?> Items { private set; get; }

        public double? this[int index]
        {
            get { return Items[index]; }
        }

        public ItemSet(IEnumerable<double?> items) 
        {
            Items = items.ToList().AsReadOnly();

            foreach (var item in Items)
            {
                if (item.HasValue)
                {
                    Size++;
                }
            }
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

            return other.Size == Size && Arrays.AreEquals(other.Items, Items);
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
                return (Size * 397) ^ (Items != null ? Arrays.GetHashCodeForArray(Items) : 0);
            }
        }

        public static double ConfidenceForRule(int premiseCount, int consequenceCount)
        {
            return (double)premiseCount / (double)consequenceCount;
        }

        public override string ToString()
        {
            return string.Format("Size: {0}, Items: {1}", Size, Arrays.ArrayToString(Items));
        }
    }

}
