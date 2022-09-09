namespace PuzzleCubes
{
    namespace Models
    {
        using System;
        
        using Newtonsoft.Json;
        using Newtonsoft.Json.Converters;

        [Serializable]
        public class CubeState : BaseData
        {

            [UnityEngine.SerializeField] private string id;

            public enum State
            {
                UNINITIALIZED,
                INITIALIZING,
                INITIALIZED,
                ERROR
            }

            [UnityEngine.SerializeField] private State current;
            [UnityEngine.SerializeField] private Pose pose;
          
            [UnityEngine.SerializeField] private Neighbourhood neighbourhood;
            [UnityEngine.SerializeField] private Light light;
            [UnityEngine.SerializeField] private Audio audio;
            [UnityEngine.SerializeField] private Display display;
            // [UnityEngine.SerializeField] private System system;

            public string Id { get => id; set => id = value; }
            public Light Light { get => this.light; set => this.light = value; }
            public Audio Audio { get => this.audio; set => this.audio = value; }
            public Display Display { get => this.display; set => this.display = value; }
            public Pose Pose { get => pose; set => pose = value; }
            public State Current { get => current; set => current = value; }
            // public System System { get => this.system; set => this.system = value; }












        }

        public class Neighbourhood
        {

        }

        public class Light
        {

        }

        public class Audio
        {

        }

        public class Display
        {

        }

        // public class System
        // {

        // }
    }

}