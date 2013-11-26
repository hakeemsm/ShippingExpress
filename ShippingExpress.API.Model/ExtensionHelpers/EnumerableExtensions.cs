using System;
using System.Collections.Generic;

namespace ShippingExpress.API.Model.ExtensionHelpers
{
    public static class EnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        public static void AddTo<T>(this ICollection<T> destination, IEnumerable<T> source)
        {
            foreach (T item in source)
            {
                destination.Add(item);
            }
        }
    }
}
