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

            // public static JsonDatagram Create(List<BaseData> items)
            // {
            //     JsonDatagram datagram = new JsonDatagram();

            //     foreach(BaseData d in items)
            //     {
            //         string className = d.GetType().Name;
            //         string jsonKey = char.ToLower(className[0]) + className.Substring(1);
            //         var json = JsonConvert.SerializeObject(d, Formatting.Indented, new JsonSerializerSettings
            //         {
            //             NullValueHandling = NullValueHandling.Ignore,
            //             TypeNameHandling = TypeNameHandling.Objects
            //         });
                  
            //         datagram.TokenData.Add(jsonKey, json);

            //     }
            //     return datagram;
            // }
        }
    }
}