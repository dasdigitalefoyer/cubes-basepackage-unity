
using System.Runtime.Serialization;

using UnityEngine;

namespace PuzzleCubes
{
    using Models;
    namespace Communication
    {
        [RequireComponent(typeof(CommunicationManager))]
        [RequireComponent(typeof(EventDispatcher))]
        public class TestCommunication : MonoBehaviour

        {

            public CommunicationManager communication;
            public EventDispatcher dispatcher;
            public enum DataType
            {
                [EnumMember(Value = "CubeControl")]
                CubeControl,
                [EnumMember(Value = "CubeState")]
                CubeState,
                [EnumMember(Value = "Notification")]
                Notification,
                [EnumMember(Value = "None")]
                None
            }
            [Header("Test Data")]
            public Notification notification = new Notification();
       
            public CubeControl cubeControl = new CubeControl();

            public CubeState cubeState = new CubeState();

            public DataType requestTestDataType = DataType.None;

            [Header("External Comnunication Tests")]
            [InspectorButton("SendNotification")]
            public bool sendNotification;


            // [InspectorButton("SendAppDatagram")]
            // public bool sendAppDatagram;

            [InspectorButton("SendCubeControl")]
            public bool sendCubeControl;

            [InspectorButton("RequestTestData")]
            public bool requestTestData;

            [Header("Internal Comnunication Tests")]
            [InspectorButton("SendNotificationInternal")]
            public bool sendNotificationInternal;


            // 

            [InspectorButton("SendCubeControlInternal")]
            public bool sendCubeControlInternal;

            [InspectorButton("SendCubeStateInternal")]
            public bool sendCubeStateInternal;

          

          
            // Start is called before the first frame update
            void Start()
            {
                Debug.Log("Start");
                communication = GetComponent<CommunicationManager>();
                dispatcher = GetComponent<EventDispatcher>();

            }

            // Update is called once per frame
            void Update()
            {

            }

            private void SendNotification()
            {
                if (Application.isPlaying)
                {
                    Debug.Log("SendNotification");
                    if (communication != null)
                    {
                        var datagram = new Models.WebSocketDatagram();
                        datagram.Notification = notification;
                        communication.SendWebSocketMessage(datagram);
                    }
                }
            }

            // private void SendAppDatagram()
            // {
            //     if (Application.isPlaying)
            //     {
            //         Debug.Log("SendAppDatagram");
            //         if (communication != null)
            //         {
            //             var datagram = new Models.WebSocketDatagram();
            //             datagram.AppDatagram = new Models.AppDatagram();
            //             communication.SendWebSocketMessage(datagram);
            //         }
            //     }
            // }

            private void SendCubeControl()
            {
                if (Application.isPlaying)
                {
                    Debug.Log("SendCubeControl");
                    if (communication != null)
                    {
                        var datagram = new Models.WebSocketDatagram();
                        datagram.CubeControl = cubeControl;

                        communication.SendWebSocketMessage(datagram);
                    }
                }
            }

            private void RequestTestData()
            {
                if (Application.isPlaying)
                {
                    Debug.Log("RequestTestData");
                    if (communication != null)
                    {
                        var datagram = new Models.WebSocketDatagram();
                        datagram.RequestTestData = new Models.RequestTestData();

                        datagram.RequestTestData.Type = requestTestDataType.ToString();
                        communication.SendWebSocketMessage(datagram);
                    }
                }
            }


             private void SendNotificationInternal()
            {
                if (Application.isPlaying)
                {
                    Debug.Log("SendNotificationInternal");
                    if (dispatcher != null)
                    {
                        var datagram = new Models.WebSocketDatagram();
                        datagram.Notification = notification;
                        dispatcher.HandleWebsocketEvent(datagram);
                    }
                }
            }

            private void SendCubeStateInternal()
            {
                if (Application.isPlaying)
                {
                    Debug.Log("SendCubeStateInternal");
                    if (dispatcher != null)
                    {
                        var datagram = new Models.WebSocketDatagram();
                        datagram.CubeState = cubeState;
                        dispatcher.HandleWebsocketEvent(datagram);
                    }
                }
            }

            // private void SendAppDatagramInternal()
            // {
            //     if (Application.isPlaying)
            //     {
            //         Debug.Log("SendAppDatagramInternal");
            //         if (dispatcher != null)
            //         {
            //             var datagram = new Models.WebSocketDatagram();
            //             datagram.AppDatagram = new Models.AppDatagram();
            //             dispatcher.HandleWebsocketEvent(datagram);
            //         }
            //     }
            // }

            private void SendCubeControlInternal()
            {
                if (Application.isPlaying)
                {
                    Debug.Log("SendCubeControlInternal");
                    if (dispatcher != null)
                    {
                        var datagram = new Models.WebSocketDatagram();
                        datagram.CubeControl = cubeControl;

                        dispatcher.HandleWebsocketEvent(datagram);
                    }
                }
            }


         
        }

    }
}