
using NativeWebSocket;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

namespace PuzzleCubes
{
    using Models;
    namespace Communication
    {

        public class CommunicationManager : MonoBehaviour
        {
            WebSocket websocket;

            public string host = "pc-server";
            public string path = "/ws";
            public int port = 8888;


            public WebSocketEvent websocketEvent;

            // Start is called before the first frame update
            async void Start()
            {

                websocket = new WebSocket("ws://" + host + ":" + port + path);

                websocket.OnOpen += () =>
                {
                    Debug.Log("Connection open!");
                };

                websocket.OnError += (e) =>
                {
                    Debug.Log("Error! " + e);
                };

                websocket.OnClose += (e) =>
                {
                    Debug.Log("Connection closed!");

                };

                websocket.OnMessage += (bytes) =>
                {
                    Debug.Log("OnMessage!");
                    //    Debug.Log((string)bytes);

                    // getting the message as a string
                    var message = System.Text.Encoding.UTF8.GetString(bytes);
                    Debug.Log("OnMessage! " + message);
                    WebSocketDatagram data = JsonConvert.DeserializeObject<WebSocketDatagram>(message);
                    websocketEvent.Invoke(data);
                };

                // Keep sending messages at every 0.3s
                //InvokeRepeating("SendWebSocketMessage", 0.0f, 1f);

                // waiting for messages
                await ContinousConnect();
                Debug.Log("Start end!");
            }

            async System.Threading.Tasks.Task ContinousConnect()
            {
                while (!this.IsDestroyed())
                {
                    await websocket.Connect();
                    await System.Threading.Tasks.Task.Delay(1000);
                    Debug.Log("Reconnect");
                }

                Debug.Log("ContinousConnect stopped");
            }

            void Update()
            {

#if !UNITY_WEBGL || UNITY_EDITOR
                websocket.DispatchMessageQueue();
#endif
            }

            public async void SendWebSocketMessage(WebSocketDatagram datagram)
            {
                Debug.Log("SendWebSocketMessage");
                if (websocket.State == WebSocketState.Open)
                {
                    var json = JsonConvert.SerializeObject(datagram, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                    Debug.Log(json);
                    // Sending bytes
                    //await websocket.Send(new byte[] { 10, 20, 30 });
                    //await websocket.Send("Test");

                    // Sending plain text
                    await websocket.SendText(json);
                }
            }

            private async void OnApplicationQuit()
            {
                if(websocket != null)
                    await websocket.Close();
            }
        }
    }
}