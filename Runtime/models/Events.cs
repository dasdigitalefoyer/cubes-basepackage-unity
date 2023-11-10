
namespace PuzzleCubes
{
    namespace Models
    {

        using UnityEngine;
        using UnityEngine.Events;
        using Newtonsoft.Json.Linq;
        using MQTTnet;
        using System.Collections.Generic;


        [System.Serializable]
        public class NotificationEvent : UnityEvent<Notification>
        {
        }

        [System.Serializable]
        public class AppDatagramEvent : UnityEvent<string>
        {
        }

        [System.Serializable]
        public class WebSocketEvent : UnityEvent<WebSocketDatagram>
        {
        }

        [System.Serializable]
        public class JsonEvent : UnityEvent<JsonDatagram>
        {
        }

        [System.Serializable]
        public class CubePoseEvent : UnityEvent<CubePose>
        {
        }

        [System.Serializable]
        public class CubeControlEvent : UnityEvent<CubeControl>
        {
        }

        [System.Serializable]
        public class CubeStateEvent : UnityEvent<CubeState>
        {
        }

       

        [System.Serializable]
        public class AppStateEvent : UnityEvent<AppState>
        {
        }

        [System.Serializable]
        public class ValidConnectionEvent : UnityEvent<ValidConnection>
        {
        }

        [System.Serializable]
        public class DebugControlEvent : UnityEvent<DebugControl>
        {
        }

        [System.Serializable]
        public class MqttEvent : UnityEvent<MqttApplicationMessage,  IList<string>> {
	    }

        [System.Serializable]
        public class FloatEvent : UnityEvent<float> {
	    }

        [System.Serializable]
        public class StringEvent : UnityEvent<string> {
	    }

        [System.Serializable]
        public class BoolEvent : UnityEvent<bool> {
	    }

        [System.Serializable]
        public class IntEvent : UnityEvent<int> {
	    }
    }

}