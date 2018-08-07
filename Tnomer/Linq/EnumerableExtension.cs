using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Tnomer.Linq
{
    public static class EnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> e, Action<T> action)
        {
            foreach( var item in e)
            {
                action.Invoke(item);
            }
        }

        public static Collection<TResult> ForEachToCollection<T,TResult>(this IEnumerable<T> e, Func<T, TResult> func)
        {
            var c = new Collection<TResult>();

            foreach (var item in e)
            {
                c.Add(func.Invoke(item));
            }

            return c;
        }
    }
}
