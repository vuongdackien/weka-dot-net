using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;
using System.Runtime.CompilerServices;

namespace Weka.NET.Associations
{
    /// <summary>
    /// Fill description...
    /// <remarks="This is the C# port for ItemSet.java originally developed by Eibe Frank"/>
    /// </summary>
    public class ItemSet
    {
        public IList<int?> Items { private set; get; }

        public int? this[int index]
        {
            get { return Items[index]; }
        }

        public ItemSet(IEnumerable<int?> items) 
        {
            Items = items.ToList().AsReadOnly();
        }

        public override bool Equals(object other)
        {
            if (other is ItemSet)
            {
                var otherItemSet = other as ItemSet;

                return Enumerable.SequenceEqual(Items, otherItemSet.Items);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 13 * RuntimeHelpers.GetHashCode(Items);
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
