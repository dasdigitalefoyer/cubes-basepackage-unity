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
            [UnityEngine.SerializeField] private DateTime dateTime = DateTime.Now;

            public DateTime DateTime
            {
                get { return dateTime; }
                set { dateTime = value; }
            }

        }
    }
}