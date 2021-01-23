using System;
using System.Collections.Generic;
using System.Linq;

namespace GameManager.Utils
{
    public static class CustomLinqExtension
    {
        public static List<List<T>> Split<T>(this IEnumerable<T> source,int size)
        {
            var list = new List<List<T>>();
            var sourceLists = source.ToList();
            for (int i = 0; i < source.Count(); i += size)
            {   
                
                list.Add(sourceLists.GetRange(i, Math.Min(size, source.Count() - i)));
            }

            return list;
        }
    }
}