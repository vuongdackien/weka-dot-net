using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weka.NET.Utils
{
    public static class Arrays
    {
        internal static bool Equals(double?[] left, double?[] right)
        {
            if (left.Length != right.Length)
            {
                return false;
            }

            for (int i = 0; i < left.Length; i++)
            {
                if (left[i].HasValue)
                {
                    if (false == right[i].HasValue)
                    {
                        return false;
                    }
                }
                else
                {
                    if (right[i].HasValue)
                    {
                        return false;
                    }

                    continue;
                }

                if (false == left[i].Value.Equals(right[i].Value))
                {
                    return false;
                }
            }

            return true;
        }

        internal static IEnumerable<double?> BuildSingleton(int length, int index, double? value)
        {
            double?[] values = new double?[length];

            values[index] = value;

            return values;
        }

        internal static bool AreEquals(IList<double?> left, IList<double?> right)
        {
            if (left.Count != right.Count)
            {
                return false;
            }

            for (int i = 0; i < left.Count; i++)
            {
                if (left[i].HasValue)
                {
                    if (false == right[i].HasValue)
                    {
                        return false;
                    }

                    if (false == left[i].Value.Equals(right[i].Value))
                    {
                        return false;
                    }

                    continue;
                }

                if (right[i].HasValue)
                {
                    return false;
                }
            }

            return true;
        }

        internal static string ArrayToString(IList<double?> Items)
        {
            var buff = new StringBuilder("[");

            foreach (var item in Items)
            {
                if (item.HasValue)
                {
                    buff.Append(item.Value);
                }

                buff.Append(",");
            }

            buff.Replace(',', ']', buff.Length - 1, buff.Length);

            return buff.ToString();
        }

        internal static int GetHashCodeForArray(IList<double?> Items)
        {
            int hash = 13;

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].HasValue)
                {
                    hash ^= Items[i].Value.GetHashCode();
                }
            }

            return hash;
        }
    }
}
