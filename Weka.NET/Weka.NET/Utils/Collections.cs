using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weka.NET.Utils
{
    public static class Collections
    {
        public static void AddAll<T>(this HashSet<T> hashSet, IEnumerable<T> elements)
        {
            foreach (var element in elements)
            {
                hashSet.Add(element);
            }
        }
    }
}
