using System;
using System.Collections.Generic;
using System.Linq;

namespace GoldDiff.Shared.Utility
{
    public static class MultiSet
    {
        public static IEnumerable<TItemType> Difference<TItemType>(IEnumerable<TItemType>? a, IEnumerable<TItemType>? b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            var result = new List<TItemType>(a);

            foreach (var item in b)
            {
                result.Remove(item);
            }

            return result;
        }
    }
}