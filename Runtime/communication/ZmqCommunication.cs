

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using UnityEngine;

namespace PuzzleCubes
{
    using System.Collections;
    using System.Threading;
    using Models;

    // using Newtonsoft.Json.Serialization;

    namespace ZmqCommunication
    {


        public class ZmqCommunication : MonoBehaviour
        {


            ConcurrentQueue<JsonDatagram> pendingJsonDatagrams = new ConcurrentQueue<JsonDatagram>();

            public JsonEvent jsonEvent;


            public string host = "pc-server";

            public int port = 5555;


            private Thread receiveThread;
            private bool running;



            public void Start()
            {
                receiveThread = new Thread((object queue) =>
                {
                    using (var socket = new PullSocket())
                    {
                        socket.Connect("tcp://localhost:5555");
                        
                        while (running)
                        {
                            // socket.SendFrameEmpty();
                            string data = socket.ReceiveFrameString();
                            //  var data = System.Text.Encoding.UTF8.GetString(args.ApplicationMessage.Payload);

                            Debug.Log("got data: " + data);
                            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonDatagram>(data);
                            if(result != null)
                                // pendingJsonDatagrams.Enqueue(result);
                                (queue as ConcurrentQueue<JsonDatagram>).Enqueue(result);
                            
                        }
                    }
                });
                running = true;
                receiveThread.Start(this.pendingJsonDatagrams);

                StartCoroutine(ProcessEventQueue());
            }

            public void Stop()
            {
                running = false;
                receiveThread.Join();
            }


            // managedMqttClient.ApplicationMessageReceivedAsync +=  async (args) => 
            // {
            // //    Debug.Log("got mqtt message: " +args.ToString());
            //     if(args.ApplicationMessage.Topic.Equals(Topic.globalAppState) || (args.ApplicationMessage.Topic.Equals(Topic.dedicatedAppState(this.clientId)) ) )
            //     {
            //         var data = System.Text.Encoding.UTF8.GetString(args.ApplicationMessage.Payload);

            //         Debug.Log("appState: " + data);
            //         var result = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonDatagram>(data);
            //         if(result != null)
            //             pendingJsonDatagrams.Enqueue(result);


            //     }
            //     await  Task.CompletedTask;

            // };




            // Start is called before the first frame update

           

            IEnumerator ProcessEventQueue()
            {
                while (true)
                {
                    JsonDatagram jd;
                    while (pendingJsonDatagrams.TryDequeue(out jd))
                    {
                        jsonEvent.Invoke(jd);
                    }
                    yield return new WaitForEndOfFrame();
                }

            }



            private void OnApplicationQuit()
            {
                Stop();
            }
        }
    }
}