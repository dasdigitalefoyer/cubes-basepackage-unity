#nullable enable
namespace PuzzleCubes
{

    namespace Models
    {
        using Newtonsoft.Json;
        using Newtonsoft.Json.Linq;
        using Newtonsoft.Json.Serialization;
        [System.Serializable()]
        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public class WebSocketDatagram
        {
            public string Id { get; set; } ="";
            public MetaData MetaData { get; set; } = new MetaData();
            public Notification? Notification { get; set; }
            public JRaw? AppDatagram { get; set; }

            public CubeControl? CubeControl { get; set; }
            public CubeState? CubeState { get; set; }

            public RequestTestData? RequestTestData { get; set; }

            public JRaw?  AppDatagram2 { get; set;}
        }
    }
}
