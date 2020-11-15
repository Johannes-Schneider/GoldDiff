using System;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLClientEventConverter : JsonConverter<LoLClientEvent>
    {
        private LoLClientEventTypeConverter EventTypeConverter { get; } = new();

        public override void WriteJson(JsonWriter writer, LoLClientEvent? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override LoLClientEvent ReadJson(JsonReader reader, Type objectType, LoLClientEvent? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // taken from https://blog.mbwarez.dk/deserializing-different-types-based-on-properties-with-newtonsoft-json/
            var jsonObject = JToken.ReadFrom(reader);
            
            var eventTypeReader = jsonObject["EventName"]!.CreateReader();
            if (!eventTypeReader.Read())
            {
                throw new Exception($"Unable to read the {nameof(LoLClientEventType)}!");
            }
            
            var eventType = (LoLClientEventType) EventTypeConverter.ReadJson(eventTypeReader, typeof(string), null, serializer)!;
            var result = eventType.CreateEvent();
            serializer.Populate(jsonObject.CreateReader(), result);
            return result;
        }
    }
}