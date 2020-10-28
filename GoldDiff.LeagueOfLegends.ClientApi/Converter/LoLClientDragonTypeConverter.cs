using System;
using System.Reflection;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using log4net;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLClientDragonTypeConverter : ReadOnlyConverter<string>
    {
        private static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
                Log.Error($"Unable to convert {reader.Value} ({reader.ValueType?.Name}) to {nameof(LoLClientDragonType)}!");
                return LoLClientDragonType.Undefined;
            }

            if (value.Equals("Fire", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientDragonType.Fire;
            }
            
            if (value.Equals("Water", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientDragonType.Water;
            }
            
            if (value.Equals("Air", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientDragonType.Wind;
            }
            
            if (value.Equals("Earth", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientDragonType.Earth;
            }
            
            if (value.Equals("Elder", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientDragonType.Elder;
            }

            Log.Error($"Unable to convert {value} to {nameof(LoLClientDragonType)}!");
            return LoLClientDragonType.Undefined;
        }
    }
}