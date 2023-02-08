namespace PuzzleCubes
{

    namespace Models
    {
        using System.Collections.Generic;
        using Newtonsoft.Json;
        using Newtonsoft.Json.Linq;
        using Newtonsoft.Json.Serialization;
        [System.Serializable]
        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public class JsonDatagram
        {
            [JsonExtensionData]
            public IDictionary<string,JToken> TokenData {get;set;}

            public static JsonDatagram CreateFrom(List<BaseData> items)
            {
                JsonDatagram datagram = new JsonDatagram();

                foreach(BaseData d in items)
                {
                    string className = d.GetType().Name;
                    string jsonKey = char.ToLower(className[0]) + className.Substring(1);
                    JToken t = JToken.FromObject(d);
                  
                    datagram.TokenData.Add(jsonKey, t);

                }
                return datagram;

              
            }

            public static JsonDatagram CreateFrom(BaseData data)
            {
                JsonDatagram datagram = new JsonDatagram();

                
                string className = data.GetType().Name;
                string jsonKey = char.ToLower(className[0]) + className.Substring(1);
                JToken t = JToken.FromObject(data);
                
                datagram.TokenData.Add(jsonKey, t);
                return datagram;

              
            }
        }
    }
}