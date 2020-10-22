using System;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLClientEventConverter : JsonConverter
    {
        private LoLClientEventTypeConverter EventTypeConverter { get; } = new LoLClientEventTypeConverter();

        public override bool CanWrite => false;
        public override bool CanRead => true;

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new InvalidOperationException();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
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

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}