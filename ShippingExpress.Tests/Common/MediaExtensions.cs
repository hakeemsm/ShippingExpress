using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace ShippingExpress.Tests.Common
{
    public static class MediaExtensions
    {
        internal static IEnumerable<MediaTypeWithQualityHeaderValue> ToMediaTypeWithQualityHeaderValues(
            this IEnumerable<string> source)
        {
            return source.Select(item => new MediaTypeWithQualityHeaderValue(item));
        }
    }
}
