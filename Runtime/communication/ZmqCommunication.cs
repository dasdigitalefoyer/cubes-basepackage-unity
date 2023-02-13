

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

    namespace Communication
    {


        public class ZmqCommunication : MonoBehaviour
        {

            private Queue<string> outgoingMessages = new Queue<string>();
            ConcurrentQueue<JsonDatagram> pendingJsonDatagrams = new ConcurrentQueue<JsonDatagram>();

            public JsonEvent jsonEvent;


            public string host = "localhost";

            public int port = 5555;


            private Thread receiveThread;
            private bool running;
            PairSocket socket; // = new PullSocket();
           

            public void Start()
            {
                Debug.Log("starting ZMQ");
                
            
                receiveThread = new Thread((object queue) =>
                {
                    
                    AsyncIO.ForceDotNet.Force();
                    using (socket = new PairSocket())
                    {
                        socket.Connect($"tcp://{host}:{port}");
                        running = true;
                        while (running)
                        {
                            // socket.SendFrameEmpty();
                            // string data;
                            try
                            {
                                if (socket.TryReceiveFrameString(out var data))
                                {
                                    // Debug.Log("got data: " + data);

                                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonDatagram>(data);
                                    if (result != null)
                                        // pendingJsonDatagrams.Enqueue(result);
                                        (queue as ConcurrentQueue<JsonDatagram>).Enqueue(result);
                                }
                                //  var data = System.Text.Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
                            }

                            catch (Exception e) { Debug.LogError(e.Message);}


                        }
                        socket.Close();
                        NetMQConfig.Cleanup();
                    }
                });
                
                receiveThread.Start(this.pendingJsonDatagrams);

                StartCoroutine(ProcessEventQueue());
            }

            public void Stop()
            {
                Debug.Log("stop ZMQ");
                running = false;
                receiveThread.Join();
                socket.Close();
                NetMQConfig.Cleanup();
                // NetMQConfig.Cleanup();

                Debug.Log("ZMQ stopped");
            }





           

            IEnumerator ProcessEventQueue()
            {
                while (true)
                {
                   
                    JsonDatagram jd;
                    while (pendingJsonDatagrams.TryDequeue(out jd))
                    {
                        // Debug.Log(jd);
                        jsonEvent.Invoke(jd);
                    }
                    while (socket != null && outgoingMessages.TryDequeue(out var m))
                    {
                        
                        socket.SendFrame(m);
                    }
                    yield return new WaitForEndOfFrame();
                }

            }

            // void OnDestroy()
            // {

            //     if(receiveThread != null && receiveThread.IsAlive)
            //         Stop();
            // }

            private void OnDestroy()
            {
                if(receiveThread != null && receiveThread.IsAlive)
                    Stop();
               

                
            }

           
            public async void Send(string message , bool enqueue = false)
            {
                
               
                if(socket != null)
                {
                    Debug.Log("Send to ZMQ");
                    socket.SendFrame(message);
                }
                else if(enqueue)
                {
                    outgoingMessages.Enqueue(message);
                }
                    
                await Task.CompletedTask;
            
            }
        }
    }
}