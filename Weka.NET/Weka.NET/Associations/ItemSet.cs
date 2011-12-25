namespace Weka.NET.Associations
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Weka.NET.Lang;
    using Weka.NET.Utils;
    using System.Diagnostics.Contracts;
    using Weka.NET.Core;

    [Immutable]
    [Serializable]
    public sealed class ItemSet : IEquatable<ItemSet>
    {
        readonly IList<double?> values;

        public int Size { get; private set; }

        public int Length { get { return values.Count; } }

        public ItemSet(params double?[] values) : this(values.ToList())
        {
        }

        public ItemSet(IList<double?> values)
        {
            this.values = new List<double?>(values);

            Size = (from v in values where v.HasValue select v).Count();
        }

        public double? this[int index]
        {
            get { return values[index]; }
        }

        public ItemSet Union(ItemSet other)
        {
            if (values.Count != other.values.Count)
            {
                throw new ArgumentException("");
            }

            var newValues = new double?[values.Count];

            for(int i=0;i<other.values.Count;i++)
            {
                if (values[i].HasValue && false == other.values[i].HasValue)
                {
                    newValues[i] = values[i];
                }

                if (false == values[i].HasValue && other.values[i].HasValue)
                {
                    newValues[i] = other.values[i];
                }

                if (values[i].HasValue && other.values[i].HasValue)
                {
                    if (false == values[i].Equals(other.values[i]))
                    {
                        throw new ArgumentException("Can't union");
                    }

                    newValues[i] = other.values[i];
                }
            }

            return new ItemSet(newValues);
        }

        public int IntersectsCount(ItemSet other)
        {
            int intersects = 0;

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].HasValue && other.values[i].HasValue
                    && values[i].Equals(other.values[i]) )
                {
                    intersects++;
                }
            }

            return intersects;
        }

        public bool IsIntersecteable(ItemSet other)
        {
            if (values.Count != other.values.Count)
            {
                return false;
            }

            for (int i = 0; i < other.values.Count; i++)
            {
                if (values[i].HasValue && false == other.values[i].HasValue)
                {
                    continue;
                }

                if (false == values[i].HasValue && other.values[i].HasValue)
                {
                    continue;
                }

                if (values[i].HasValue && other.values[i].HasValue && false == values[i].Equals(other.values[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public bool ContainedBy(Instance instance)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].HasValue)
                {
                    if (false == values[i].Equals(instance[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        public bool Equals(ItemSet other)
        {
            return values.ListEquals(other.values);
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
                return values.GetHashCodeForArray();
            }
        }

        public override string ToString()
        {
            return "Items[Size= " + Size + ", values=" + values.ListToString() + "]";
        }

    }
}
