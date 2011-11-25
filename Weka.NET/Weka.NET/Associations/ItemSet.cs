using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;
using System.Runtime.CompilerServices;

namespace Weka.NET.Associations
{
    public class ItemSetComparer<ItemSet> : IEqualityComparer<ItemSet>
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
        private List<ItemSet> combineds;

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

        public bool Equals(ItemSet other)
        {
            var otherItemSet = other as ItemSet;

            if (Items.Count() != otherItemSet.Items.Count())
            {
                return false;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].HasValue)
                {
                    if (otherItemSet.Items[i].HasValue)
                    {
                        if (Items[i].Value.Equals(otherItemSet.Items[i].Value))
                        {
                            continue;
                        }
                    }
                    return false;
                }

                if (otherItemSet.Items[i].HasValue)
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object other)
        {
            if (other is ItemSet)
            {
                return Equals(other as ItemSet);
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 1;

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].HasValue)
                {
                    hash *= Items[i].Value.GetHashCode();
                }
            }

            return 13 * hash;
        }

        public static double ConfidenceForRule(int premiseCount, int consequenceCount)
        {
            return (double)premiseCount / (double)consequenceCount;
        }

        public override string ToString()
        {
            var buff = new StringBuilder("ItemSet{items:[");
            
            foreach (var item in Items)
            {
                buff.Append(item.HasValue ? item.Value.ToString() : "null");
                buff.Append(",");
            }
            buff.Replace(',', ']', buff.Length - 1, 1);

            buff.Append("}");

            return buff.ToString();
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
    }

}
