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
        public static int MaxIndex(this double[] array)
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
    }
}
