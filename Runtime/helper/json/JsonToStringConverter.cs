using Newtonsoft.Json;        
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
class JsonToStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(JTokenType));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Object)
            {
                return token.ToString();
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {


            //serialize as actual JSON and not string data
            var token = JToken.Parse(value.ToString());
            writer.WriteToken(token.CreateReader());

        }
    }