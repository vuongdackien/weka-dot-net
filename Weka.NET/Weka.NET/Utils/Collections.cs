namespace Weka.NET.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    public static class Collections
    {
        public static void AddAll<T>(this HashSet<T> hashSet, IEnumerable<T> elements)
        {
            foreach (var element in elements)
            {
                hashSet.Add(element);
            }
        }

        public static bool ListEquals<T>(this IList<T> list, IList<T> other)
        {
            if (list.Count != other.Count)
            {
                return false;
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null) 
                {
                    if (other[i] != null)
                    {
                        return false;
                    }

                    continue;
                }

                if (false == list[i].Equals(other[i]))
                {
                    return false;
                }
            }

            return true;

        }

        public static string ListToString<T>(this IList<T> list)
        {
            var buff = new StringBuilder("[");

            foreach (var e in list) buff.Append(e.ToString()).Append(",");

            
            return buff.ToString();
        }

    }
}
