using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PuzzleCubes
{
    namespace Models
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum Edge
        {
            FORWARD,
            BACKWARD,
            LEFT,
            RIGHT,
        }
    }
}