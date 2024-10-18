using System.Collections.Generic;

namespace DefaultNamespace
{
    public static class Random
    {
        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}