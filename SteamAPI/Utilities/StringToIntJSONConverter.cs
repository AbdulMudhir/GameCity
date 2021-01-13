using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;



namespace SteamAPI.Utilities
{
    public class StringToIntJSONConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            if (token.Type == JTokenType.String)
            {

                return int.Parse(token.Value<string>());
            }

            var tokens = token.Value<int>();

            return tokens;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
