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
    }
}
