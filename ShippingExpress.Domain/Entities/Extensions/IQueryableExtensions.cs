using System;
using System.Collections.Generic;
using System.Linq;
using ShippingExpress.Domain.Entities.Core;

namespace ShippingExpress.Domain.Entities.Extensions
{
    public static class IQueryableExtensions
    {
        public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> query, int pageIdx, int pageSize)
        {
            var totalCount = query.Count();
            var nextList = query.Skip((pageIdx - 1)*pageSize).Take(pageSize);
            return new PaginatedList<T>(pageIdx,pageSize,totalCount,nextList);
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }   
        }
    }
}
