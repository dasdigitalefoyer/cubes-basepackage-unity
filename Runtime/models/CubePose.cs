namespace PuzzleCubes
{
    namespace Models
    {
        using Newtonsoft.Json;
        using Newtonsoft.Json.Converters;
        using UnityEngine;

        [System.Serializable()]
        public class CubePose
        {

            [UnityEngine.SerializeField] private string id;
            [UnityEngine.SerializeField] private UnityEngine.Vector2 position;
            [UnityEngine.SerializeField] private UnityEngine.Vector2 relativeMotion;

            [UnityEngine.SerializeField] private CubeDirection relativeDirection;
            // [UnityEngine.SerializeField] private UnityEngine.Vector2 positionConfidence;
            // [UnityEngine.SerializeField] private UnityEngine.Vector2 velocity;
            // [UnityEngine.SerializeField] private UnityEngine.Vector2 velocityConfidence;
            [UnityEngine.SerializeField] private float orientation;
            // [UnityEngine.SerializeField] private float angularVelocity;

            [JsonConverter(typeof(helper.json.JsonVector2Converter))]
            public UnityEngine.Vector2 Position { get => position; set => position = value; }
            //  [JsonConverter(typeof(helper.json.JsonVector2Converter))]
            // public UnityEngine.Vector2 Velocity { get => velocity; set => velocity = value; }
            // public float AngularVelocity { get => angularVelocity; set => angularVelocity = value; }
            public float Orientation { get => orientation; set => orientation = value; }
            public CubeDirection RelativeDirection { get => relativeDirection; set => relativeDirection = value; }
            public Vector2 RelativeMotion { get => relativeMotion; set => relativeMotion = value; }
            public string CubeId { get => cubeId; set => cubeId = value; }
        }

    }
}