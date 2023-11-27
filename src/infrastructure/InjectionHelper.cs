using System;
using System.Collections.Generic;
using System.Linq;

namespace DependencyExtension
{
    public static class InjectionHelper
    {
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("mapFunction");
            }

            if (!source.IsNullOrEmpty())
            {
                foreach (var item in source)
                {
                    action(item);
                }
            }
        }

        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source == null || !source.Any();
        }        
    }
}
