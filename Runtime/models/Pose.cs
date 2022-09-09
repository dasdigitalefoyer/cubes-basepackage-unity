namespace PuzzleCubes
{
    namespace Models
    {
        using Newtonsoft.Json;
        using Newtonsoft.Json.Converters;

        [System.Serializable()]
        public class Pose
        {
            [UnityEngine.SerializeField] private UnityEngine.Vector2 position;
            [UnityEngine.SerializeField] private UnityEngine.Vector2 positionConfidence;
            [UnityEngine.SerializeField] private UnityEngine.Vector2 velocity;
            [UnityEngine.SerializeField] private UnityEngine.Vector2 velocityConfidence;
            [UnityEngine.SerializeField] private float orientation;
            [UnityEngine.SerializeField] private float angularVelocity;

            [JsonConverter(typeof(helper.json.JsonVector2Converter))]
            public UnityEngine.Vector2 Position { get => position; set => position = value; }
            public UnityEngine.Vector2 Velocity { get => velocity; set => velocity = value; }
            public float AngularVelocity { get => angularVelocity; set => angularVelocity = value; }
            public float Orientation { get => orientation; set => orientation = value; }
        }

    }
}