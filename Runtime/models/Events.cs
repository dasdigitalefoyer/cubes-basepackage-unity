
namespace PuzzleCubes
{
    namespace Models
    {

        using UnityEngine;
        using UnityEngine.Events;
        using Newtonsoft.Json.Linq;
        using MQTTnet;

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
        public class CubeControlEvent : UnityEvent<CubeControl>
        {
        }

        [System.Serializable]
        public class CubeStateEvent : UnityEvent<CubeState>
        {
        }

        [System.Serializable]
        public class MqttEvent : UnityEvent<MqttApplicationMessage>
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
    }

}