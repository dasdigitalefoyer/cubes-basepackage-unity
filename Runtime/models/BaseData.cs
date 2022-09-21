namespace PuzzleCubes
{

    namespace Models
    {
        using System;
        using Newtonsoft.Json;
        using Newtonsoft.Json.Serialization;
        [System.Serializable]
        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public abstract class BaseData
        {
            [UnityEngine.SerializeField] private DateTime timestamp = DateTime.Now;

            public DateTime Timestamp
            {
                get { return timestamp; }
                set { timestamp = value; }
            }

        }
    }
}