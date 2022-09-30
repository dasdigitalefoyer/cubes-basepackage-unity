
namespace PuzzleCubes
{
    namespace Models
    {

        using UnityEngine;
        using UnityEngine.Events;
        using Newtonsoft.Json.Linq;

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
        public class CubeControlEvent : UnityEvent<CubeControl>
        {
        }

        [System.Serializable]
        public class CubeStateEvent : UnityEvent<CubeState>
        {
        }

     
    }

}