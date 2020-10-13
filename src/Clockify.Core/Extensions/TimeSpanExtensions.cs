using System;
using System.Collections.Generic;
using System.Linq;

namespace Clockify.Core.Extensions
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, TimeSpan> selector)
        {
            var ts = new TimeSpan();
            return source.Aggregate(ts, (current, entry) => current + selector(entry));
        }
    }
}
