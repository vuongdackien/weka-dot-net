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
    }
}
