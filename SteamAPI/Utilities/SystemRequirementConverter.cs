

using SteamAPI.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SteamAPI.Utilities
{
    public class SystemRequirementConverter : JsonConverter<SystemRequirement>
    {

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(SystemRequirement);
        }

        public override SystemRequirement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject )
            {

                reader.Read();

                return null;

              
                
            }
            

            reader.Read();


            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string propertyName = reader.GetString();

            var sr = new SystemRequirement();

            while (reader.Read())
            {
               
                //reader.Read();

                if (reader.TokenType == JsonTokenType.PropertyName || reader.TokenType == JsonTokenType.String)
                {
                    

                    switch (propertyName)
                    {
                            
                        case "minimum":
                            sr.Minimum = reader.GetString();

                            break;
                        case "recommended":
                            sr.Recommended = reader.GetString();
                            break;

                    }
              
                 
                }
                reader.Read();


                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return sr;
                }
              
                 propertyName = reader.GetString();
             



            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, SystemRequirement value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
