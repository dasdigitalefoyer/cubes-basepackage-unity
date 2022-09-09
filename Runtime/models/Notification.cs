namespace PuzzleCubes
{
    namespace Models
    {
        using System;
        using Newtonsoft.Json;
        using Newtonsoft.Json.Converters;
        [Serializable]
        public class Notification : BaseData
        {
            public enum TypeEnum
            {
                [System.Runtime.Serialization.EnumMember(Value = "INFO")]
                INFO,
                [System.Runtime.Serialization.EnumMember(Value = "DEBUG")]
                DEBUG,
                [System.Runtime.Serialization.EnumMember(Value = "WARNING")]
                WARNING,
                [System.Runtime.Serialization.EnumMember(Value = "ERROR")]
                ERROR
            }


            [UnityEngine.SerializeField] private TypeEnum type = TypeEnum.INFO;
            [UnityEngine.SerializeField] private String message = "";
            [JsonConverter(typeof(StringEnumConverter))]
            public TypeEnum Type
            {
                get { return type; }
                set { type = value; }
            }
            public String Message
            {
                get { return message; }
                set { message = value; }
            }
        };
    }
}