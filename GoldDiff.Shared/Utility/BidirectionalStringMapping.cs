using System;
using System.Linq;

namespace GoldDiff.Shared.Utility
{
    public class BidirectionalStringMapping<TValue>
    {
        private TValue[] Values { get; }
        
        private string[] Names { get; }
        
        private TValue FallbackValue { get; }
        
        private string FallbackName { get; }

        public StringComparison NameComparison { get; set; } = StringComparison.InvariantCultureIgnoreCase;

        public BidirectionalStringMapping(ValueTuple<TValue, string> fallback, params ValueTuple<TValue, string>[] values)
        {
            FallbackValue = fallback.Item1;
            FallbackName = fallback.Item2;

            Values = values.Select(tuple => tuple.Item1).ToArray();
            Names = values.Select(tuple => tuple.Item2).ToArray();
        }

        public TValue Get(string? name)
        {
            for (var i = 0; i < Names.Length; ++i)
            {
                if (Names[i].Equals(name, NameComparison))
                {
                    return Values[i];
                }
            }

            return FallbackValue;
        }

        public string Get(TValue? value)
        {
            for (var i = 0; i < Values.Length; ++i)
            {
                if (Values[i]?.Equals(value) == true)
                {
                    return Names[i];
                }
            }

            return FallbackName;
        }
    }
}