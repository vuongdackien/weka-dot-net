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

        public static string ListToString<T>(this IList<T> list)
        {
            var buff = new StringBuilder("[");

            foreach (var e in list) buff.Append(e.ToString()).Append(",");

            
            return buff.ToString();
        }

        internal static IList<Associations.AssociationRule> EmptyList<T1>()
        {
            throw new NotImplementedException();
        }
    }
}
