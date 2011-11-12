using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;

namespace Weka.NET.Associations
{
    /// <summary>
    /// Fill description...
    /// <remarks="This is the C# port for ItemSet.java originally developed by Eibe Frank"/>
    /// </summary>
    public class ItemSet
    {
        public static IEnumerable<ItemSet> BuildSingletons(params Instance[] instances)
        {
            return null;
        }

        public IList<int?> Items { private set; get; }

        public int Counter { private set; get; }

        public int? this[int index]
        {
            get { return Items[index]; }
        }

        public ItemSet(int counter, IEnumerable<int?> items)
        {
            Counter = counter;
            Items = items.ToList().AsReadOnly();
        }

        public static double ConfidenceForRule(ItemSet premise, ItemSet consequence)
        {
            return (double)consequence.Counter / (double)premise.Counter;
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
