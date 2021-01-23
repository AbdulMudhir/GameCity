using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;



namespace Webservices.JsonConverters
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
                var content = token.Value<string>();

                int conversion = 0;

                int.TryParse(content, out conversion);


                return conversion;
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
