using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Weka.NET.Tests")]
namespace Weka.NET.Lang
{
    internal static class Arrays
    {
        internal static bool ArrayEquals(double?[] left, double?[] right)
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

        internal static bool ArrayEquals(this double[] left, double[] right)
        {
            if (left.Length != right.Length)
            {
                return false;
            }

            for (int i = 0; i < left.Length; i++)
            {
                if (false == left[i].Equals(right[i]))
                {
                    return false;
                }
            }

            return true;
        }

        internal static int MaxIndex(this double[] array)
        {
            int maxIdx = 0;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[maxIdx] < array[i])
                {
                    maxIdx = i;
                }
            }

            return maxIdx;
        }

        internal static double[] Normalize(this double[] array)
        {
            double sum = array.Sum();

            if (sum == 0)
            {
                return array.Clone() as double[];
            }

            double[] normalized = new double[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                normalized[i] = array[i] / sum;
            }
  
            return normalized;
        }

        internal static string ArrayToString(this double[] counts)
        {
            if (counts.Length == 0)
            {
                return "[]";
            }

            if (counts.Length == 1)
            {
                return "[" + counts[0] + "]";
            }

            var buff = new StringBuilder("[").Append(counts[0]);

            for (int i = 1; i < counts.Length; i++)
            {
                buff.Append(", ").Append(counts[i]);
            }

            buff.Append("]");

            return buff.ToString();
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

        internal static int GetHashCodeForArray(this IList<double?> Items)
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

        internal static IList<double?> Merge(IList<double?> first, IList<double?> second)
        {
            double?[] result = new double?[first.Count];

            for (int i = 0; i < first.Count; i++)
            {
                if (first[i].HasValue)
                {
                    if (second[i].HasValue)
                    {
                        throw new Exception("booh");
                    }

                    result[i] = first[i];
                    continue;
                }

                if (second[i].HasValue)
                {
                    result[i] = second[i];
                }
            }

            return result.ToList();
        }

    }
}
