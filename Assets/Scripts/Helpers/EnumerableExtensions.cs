using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (action == null) throw new ArgumentNullException("action");

            foreach (T item in source)
            {
                action(item);
            }
        }

        public static T GetRandom<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(i => Guid.NewGuid()).First();
        }

        public static bool Rng(this float chance)
        {
            return UnityEngine.Random.Range(0f, 1f) <= chance;
        }
    }
}
