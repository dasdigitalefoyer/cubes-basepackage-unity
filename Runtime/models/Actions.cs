
namespace PuzzleCubes.Models
{
    using System.Collections.Generic;
    using MQTTnet;
    using System;

    [System.Serializable]
    public class MqttActions
    {
        public delegate void Message(MqttApplicationMessage msg, IList<string> wildcardItems= null);
       
    }
}


