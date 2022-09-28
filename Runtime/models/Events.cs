
namespace PuzzleCubes
{
    namespace Models
    {

        using UnityEngine;
        using UnityEngine.Events;

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

     
    }

}