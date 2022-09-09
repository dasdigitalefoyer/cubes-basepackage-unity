namespace PuzzleCubes
{
    namespace Models
    {
        using System;
        using Newtonsoft.Json;
        using Newtonsoft.Json.Serialization;
        [System.Serializable]
        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public class RequestTestData : BaseData
        {

            [UnityEngine.SerializeField] private String dataType;

            public String Type
            {
                get { return dataType; }
                set { dataType = value; }
            }

        };

    }
}