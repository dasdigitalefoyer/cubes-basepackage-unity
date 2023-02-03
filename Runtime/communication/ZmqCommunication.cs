

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using UnityEngine;

namespace PuzzleCubes
{
    using System;
    using System.Collections;
    using System.Net.Sockets;
    using System.Threading;
    using Models;

    // using Newtonsoft.Json.Serialization;

    namespace ZmqCommunication
    {


        public class ZmqCommunication : MonoBehaviour
        {


            ConcurrentQueue<JsonDatagram> pendingJsonDatagrams = new ConcurrentQueue<JsonDatagram>();

            public JsonEvent jsonEvent;


            public string host = "localhost";

            public int port = 5555;


            private Thread receiveThread;
            private bool running;
            PullSocket socket; // = new PullSocket();
           

            public void Start()
            {
                Debug.Log("starting ZMQ");
                //Necessary line, not sure why.
                // AsyncIO.ForceDotNet.Force();
           
            
               
                socket = new PullSocket();
                socket.Connect($"tcp://{host}:{port}");
           
                receiveThread = new Thread((object queue) =>
                {
                    
                    AsyncIO.ForceDotNet.Force();
                    using (socket = new PullSocket())
                    {
                        socket.Connect($"tcp://{host}:{port}");
                        running = true;
                        while (running)
                        {
                            // socket.SendFrameEmpty();
                            // string data;
                            if(socket.TryReceiveFrameString(System.TimeSpan.FromMilliseconds(10), out var data))
                            {
                                Debug.Log("got data: " + data);
                                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonDatagram>(data);
                                if(result != null)
                                    // pendingJsonDatagrams.Enqueue(result);
                                    (queue as ConcurrentQueue<JsonDatagram>).Enqueue(result);
                            }
                            //  var data = System.Text.Encoding.UTF8.GetString(args.ApplicationMessage.Payload);

                            
                            
                        }
                        socket.Close();
                        NetMQConfig.Cleanup();
                    }
                });
                
                // receiveThread.Start(this.pendingJsonDatagrams);

                // StartCoroutine(ProcessEventQueue());
            }

            public void Stop()
            {
                Debug.Log("stop ZMQ");
                running = false;
                // receiveThread.Join();
                // socket.Close();
                // NetMQConfig.Cleanup();
                // NetMQConfig.Cleanup();

                Debug.Log("ZMQ stopped");
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
            void Update()
            {
                // string data;
                int maxMessages = 0;
                while(socket.TryReceiveFrameString(out var data) && maxMessages < 20)
                {
                    Debug.Log("TryReceiveFrameString: " + data);
                    try
                    {
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonDatagram>(data);
                        if(result != null)
                            jsonEvent.Invoke(result);
                            // pendingJsonDatagrams.Enqueue(result);
                        
                    }
                    catch(Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                    maxMessages++;
                }
                    
            }
           

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
               
                if(receiveThread != null && receiveThread.IsAlive)
                    Stop();
                
                socket.Dispose();
                NetMQConfig.Cleanup();
                
                
            }
        }
    }
}