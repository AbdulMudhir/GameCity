using System;
using System.Collections.Generic;

namespace GameManager.Utils
{
    public static class Helper
    {
        
        public static List<List<int>> SplitList(List<int> steamAppsIds, int nSize )
        {
            var list = new List<List<int>>();

            for (int i = 0; i < steamAppsIds.Count; i += nSize)
            {
                list.Add(steamAppsIds.GetRange(i, Math.Min(nSize, steamAppsIds.Count - i)));
            }

            return list;
        }
    }
}