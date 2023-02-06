namespace PuzzleCubes
{
    namespace Models
    {
        using System;
        using Newtonsoft.Json;
        using UnityEngine;

        [Serializable]
        public class CubeControl : BaseData
        {

            [UnityEngine.SerializeField] private Vector2? position = new Vector2(0, 0);
            [UnityEngine.SerializeField] private float? orientation = 0;
            [UnityEngine.SerializeField] private Vector2? velocity = new Vector2(0, 0);
            [UnityEngine.SerializeField] private float? angularVelocity = 0;
            [UnityEngine.SerializeField] private float? positionConfidence = 0;
            [UnityEngine.SerializeField] private float? orientationConfidence = 0;
            [UnityEngine.SerializeField] private float? velocityConfidence = 0;
            [UnityEngine.SerializeField] private float? angularVelocityConfidence = 0;


            [UnityEngine.SerializeField] private bool? moving = false;
            [UnityEngine.SerializeField] private bool? tap = false;

            [UnityEngine.SerializeField] private bool? tilt = false;
            [UnityEngine.SerializeField] private bool? translationStepForward = false;
            [UnityEngine.SerializeField] private bool? translationStepBackward = false;
            [UnityEngine.SerializeField] private bool? translationStepLeft = false;
            [UnityEngine.SerializeField] private bool? translationStepRight = false;
            [UnityEngine.SerializeField] private bool? rotationStepLeft = false;
            [UnityEngine.SerializeField] private bool? rotationStepRight = false;
            [UnityEngine.SerializeField] private bool? rotationImpulseLeft = false;
            [UnityEngine.SerializeField] private bool? rotationImpulseRight = false;


            public bool? Moving { get => moving; set => moving = value; }
            public bool? Tap { get => tap; set => tap = value; }
            public bool? TranslationStepForward { get => translationStepForward; set => translationStepForward = value; }
            public bool? TranslationStepBackward { get => translationStepBackward; set => translationStepBackward = value; }
            public bool? TranslationStepLeft { get => translationStepLeft; set => translationStepLeft = value; }
            public bool? TranslationStepRight { get => translationStepRight; set => translationStepRight = value; }
            public bool? RotationStepLeft { get => rotationStepLeft; set => rotationStepLeft = value; }
            public bool? RotationStepRight { get => rotationStepRight; set => rotationStepRight = value; }
            public bool? RotationImpulseLeft { get => rotationImpulseLeft; set => rotationImpulseLeft = value; }
            public bool? RotationImpulseRight { get => rotationImpulseRight; set => rotationImpulseRight = value; }
            public bool? Tilt { get => tilt; set => tilt = value; }

            [JsonConverter(typeof(helper.json.JsonVector2Converter))]
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Vector2 Position { get => position; set => position = value; }
            public float Orientation { get => orientation; set => orientation = value; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            [JsonConverter(typeof(helper.json.JsonVector2Converter))]
<<<<<<< .mine
            public Vector2 Velocity { get => velocity; set => velocity = value; }
            
            public float AngularVelocity { get => angularVelocity; set => angularVelocity = value; }
            public float PositionConfidence { get => positionConfidence; set => positionConfidence = value; }
            public float OrientationConfidence { get => orientationConfidence; set => orientationConfidence = value; }
            public float VelocityConfidence { get => velocityConfidence; set => velocityConfidence = value; }
            public float AngularVelocityConfidence { get => angularVelocityConfidence; set => angularVelocityConfidence = value; }
=======
            public Vector2? Velocity { get => velocity; set => velocity = value; }
            public float? AngularVelocity { get => angularVelocity; set => angularVelocity = value; }
            public float? PositionConfidence { get => positionConfidence; set => positionConfidence = value; }
            public float? OrientationConfidence { get => orientationConfidence; set => orientationConfidence = value; }
            public float? VelocityConfidence { get => velocityConfidence; set => velocityConfidence = value; }
            public float? AngularVelocityConfidence { get => angularVelocityConfidence; set => angularVelocityConfidence = value; }

>>>>>>> .theirs
        }
    }

}