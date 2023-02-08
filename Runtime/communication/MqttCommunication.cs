

using Newtonsoft.Json;

using UnityEngine;
using System.Threading.Tasks;
using MQTTnet.Client;
using MQTTnet;
using MQTTnet.Protocol;
using System.Collections.Generic;
using System.Collections.Concurrent;
using MQTTnet.Extensions.ManagedClient;
using PuzzleCubes.Config;

namespace PuzzleCubes
{
    using System.Collections;
    using System.Threading;
    using Models;
    // using MQTTnet.Server;
    // using Newtonsoft.Json.Serialization;

    namespace Communication
    {

        public class MqttCommunication : MonoBehaviour
        {
          

            ConcurrentQueue<JsonDatagram> pendingJsonDatagrams = new ConcurrentQueue<JsonDatagram>();
            
            public JsonEvent jsonEvent;


            public string host = "pc-server";
            public string clientId = "";
            public int port = 1883;

            private IManagedMqttClient managedMqttClient = new MqttFactory().CreateManagedMqttClient();
           
            public async Task Connect()
            {
                if(clientId.Equals(""))
                    clientId = App.cubeId;
               

                
                // var mqttFactory = new MqttFactory();

                // using (var managedMqttClient = mqttFactory.CreateManagedMqttClient())
                {
                    var mqttClientOptions = new MqttClientOptionsBuilder()
                        .WithTcpServer(host, port)
                        .WithClientId(clientId)
                        .Build();

                    var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                        .WithClientOptions(mqttClientOptions)
                        .Build();

                    await managedMqttClient.StartAsync(managedMqttClientOptions);

                    // await managedMqttClient.SubscribeAsync("test");
                    await managedMqttClient.SubscribeAsync(App.GetGlobalAppStateTopic());
                    await managedMqttClient.SubscribeAsync(App.GetDedicatedAppStateTopic(clientId));
                   
                    

              
                    
                    managedMqttClient.ApplicationMessageReceivedAsync +=  async (args) => 
                    {
                    //    Debug.Log("got mqtt message: " +args.ToString());
                        if(args.ApplicationMessage.Topic.Equals(App.GetGlobalAppStateTopic()) || (args.ApplicationMessage.Topic.Equals(App.GetDedicatedAppStateTopic(this.clientId)) ) )
                        {
                            var data = System.Text.Encoding.UTF8.GetString(args.ApplicationMessage.Payload);

                            Debug.Log("appState: " + data);
                            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonDatagram>(data);
                            if(result != null)
                                pendingJsonDatagrams.Enqueue(result);
                            
                                
                        }
                        await  Task.CompletedTask;
                                                
                    };
                  
                    managedMqttClient.ConnectingFailedAsync  += async (args) =>
                    {
                        Debug.Log("ConnectingFailedAsync: " + args.Exception);
                        await  Task.CompletedTask;
                      
                    };
                    managedMqttClient.ConnectedAsync += async (args) =>
                    {
                        Debug.Log("Connected to Mqtt-Server");
                        await Task.CompletedTask;
                        
                    };
                   
                }
            }

            // Start is called before the first frame update
            async void Start()
            {
                await Connect();
                StartCoroutine(ProcessEventQueue());
              
                await Task.CompletedTask;
           
            }

            IEnumerator ProcessEventQueue()
            {
                while(true)
                {
                    JsonDatagram jd;
                    while(pendingJsonDatagrams.TryDequeue(out jd))
                    {
                        jsonEvent.Invoke(jd);
                    }
                    yield return new WaitForEndOfFrame();
                }
               
            }
           
            public async void Send(string topic, JsonDatagram data, MqttQualityOfServiceLevel qualityOfServiceLevel = MqttQualityOfServiceLevel.AtMostOnce, bool retain = false)
            {
                Debug.Log("Send to MQTT");
                await managedMqttClient.EnqueueAsync(topic, data.ToString(),qualityOfServiceLevel, retain);
                
            }

       
            // public async void SendWebSocketMessage(WebSocketDatagram datagram)
            // {
            //     Debug.Log("SendWebSocketMessage");
            //     if (websocket.State == WebSocketState.Open)
            //     {
            //         var json = JsonConvert.SerializeObject(datagram, Formatting.Indented, new JsonSerializerSettings
            //         {
            //             NullValueHandling = NullValueHandling.Ignore,
            //             TypeNameHandling = TypeNameHandling.Objects
            //         });
            //         Debug.Log(json);
            //         // Sending bytes
            //         //await websocket.Send(new byte[] { 10, 20, 30 });
            //         //await websocket.Send("Test");

            //         // Sending plain text
            //         await websocket.SendText(json);
            //     }
            // }

            private async void OnApplicationQuit()
            {
                Debug.Log("Stopping MqttClient");
                await managedMqttClient.StopAsync();
            }
        }
    }
}