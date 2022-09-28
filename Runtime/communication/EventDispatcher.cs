using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;

using UnityEngine;
namespace PuzzleCubes
{
    using Models;
    namespace Communication
    {
        [RequireComponent(typeof(CommunicationManager))]
        public class EventDispatcher : MonoBehaviour

        {

            public CubeControlEvent cubeControlEvent;
            public CubeStateEvent cubeStateEvent;
            public NotificationEvent notificationEvent;

            public AppDatagramEvent appDatagramEvent;


            // public CommunicationManager communication;
            // Start is called before the first frame update
            void Start()
            {
                // communication = GetComponent<CommunicationManager>();

            }

            // Update is called once per frame
            void Update()
            {

            }



            public void HandleWebsocketEvent(WebSocketDatagram data)
            {
                Debug.Log("handleWebsocketEvent!");
                if (data.Notification != null)                
                    notificationEvent.Invoke(data.Notification);
                if (data.CubeControl != null)
                    cubeControlEvent.Invoke(data.CubeControl);
                if (data.CubeState != null)
                    cubeStateEvent.Invoke(data.CubeState);
                if (data.AppDatagram != null)
                    appDatagramEvent.Invoke(data.AppDatagram.ToString());
            }
        }
    }
}