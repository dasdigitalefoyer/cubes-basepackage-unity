

using Newtonsoft.Json;

using UnityEngine;
using System.Threading.Tasks;
using MQTTnet.Client;
using MQTTnet;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using System.Collections.Generic;
using System.Collections.Concurrent;
using MQTTnet.Extensions.ManagedClient;
// using PuzzleCubes.Config;

namespace PuzzleCubes
{
    using System.Collections;
    using System.Threading;
    using Models;
    using UnityEngine.Networking.PlayerConnection;

    // using MQTTnet.Server;
    // using Newtonsoft.Json.Serialization;

    namespace Communication
    {

        public class MqttCommunication : MonoBehaviour
        {
          

            ConcurrentQueue<JsonDatagram> pendingJsonDatagrams = new ConcurrentQueue<JsonDatagram>();
            ConcurrentQueue<MqttApplicationMessage> pendingMqttMessages = new ConcurrentQueue<MqttApplicationMessage>();
            Dictionary<string, MqttActions.Message> subscriptions = new Dictionary<string, MqttActions.Message>();
            
            // public JsonEvent jsonEvent;
            // public MqttEvent mqttEvent;


            public string host = "pc-server";
            public string clientId = "";
            public int port = 1883;

            private IManagedMqttClient managedMqttClient = new MqttFactory().CreateManagedMqttClient();
           

            public async void Subscribe(string topic, MqttActions.Message a)
            {
                 await managedMqttClient.SubscribeAsync(topic);
                 this.subscriptions.Add(topic,a);
            }
            public async void Subscribe(MqttTopicFilter topicFilter, MqttActions.Message a)
            {
                 await managedMqttClient.SubscribeAsync(new List<MqttTopicFilter>(){topicFilter});
                 this.subscriptions.Add(topicFilter.Topic,a);
            }
            public async void Subscribe(IDictionary< MqttTopicFilter, MqttActions.Message>  topicFilter)
            {
                 await managedMqttClient.SubscribeAsync(topicFilter);
                 foreach(var kvp in topicFilter)
                    this.subscriptions.Add(kvp.Key.Topic,kvp.Value);
            }
            public async Task Connect()
            {
                // if(clientId.Equals(""))
                //     clientId = App.cubeId;
               
               
           
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
                    // await managedMqttClient.SubscribeAsync(App.GetGlobalAppStateTopic());
                    // await managedMqttClient.SubscribeAsync(App.GetGlobalAppStateWildcardTopic());
                    // await managedMqttClient.SubscribeAsync(App.GetDedicatedAppStateTopic(clientId));
                   
                    

              
                    
                    managedMqttClient.ApplicationMessageReceivedAsync +=  async (  args) => 
                    {
                        // MqttTopicFilterCompareResult r = MqttTopicFilterComparer.Compare(args.ApplicationMessage.Topic,App.GetGlobalAppStateWildcardTopic() );
                        // Debug.Log(r);
                        
                        
                         pendingMqttMessages.Enqueue(args.ApplicationMessage);
                       
                    // //    Debug.Log("got mqtt message: " +args.ToString());
                    //     if(args.ApplicationMessage.Topic.Equals(App.GetGlobalAppStateTopic()) || (args.ApplicationMessage.Topic.Equals(App.GetDedicatedAppStateTopic(this.clientId)) ) )
                    //     {
                    //         var data = System.Text.Encoding.UTF8.GetString(args.ApplicationMessage.Payload);

                    //         Debug.Log("appState: " + data);
                    //         var result = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonDatagram>(data);
                    //         if(result != null)
                    //             pendingJsonDatagrams.Enqueue(result);
                            
                                
                    //     }
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

            public async void Initialize(string id)
            {
                this.clientId = id;
                
                await this.Connect();
                StartCoroutine(ProcessEventQueue());
                await Task.CompletedTask;
            }

            // Start is called before the first frame update
            async void Start()
            {
                // await Connect();
                // StartCoroutine(ProcessEventQueue());
              
                await Task.CompletedTask;
           
            }

            IEnumerator ProcessEventQueue()
            {
                while(true)
                {
                //     JsonDatagram jd;
                //     while(pendingJsonDatagrams.TryDequeue(out jd))
                //     {
                //         jsonEvent.Invoke(jd);
                //     }
                    while(pendingMqttMessages.TryDequeue(out var message))
                    {
                        foreach(var kvp in subscriptions)
                        {
                            if(MqttTopicFilterComparer.Compare(message.Topic,kvp.Key) == MqttTopicFilterCompareResult.IsMatch)
                            {
                                kvp.Value( message, null);
                            }
                            
                        }
                        // if(mqttEvent != null)
                        //     mqttEvent.Invoke(message);
                    }
                    
                    yield return new WaitForEndOfFrame();
                }
               
            }
           
            public async void Send(string topic, JsonDatagram data, MqttQualityOfServiceLevel qualityOfServiceLevel = MqttQualityOfServiceLevel.AtMostOnce, bool retain = false)
            {
                Debug.Log("Send to MQTT");
                var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                
                await managedMqttClient.EnqueueAsync(topic, json,qualityOfServiceLevel, retain);
                
            }

            public async void Send(MqttApplicationMessage message)
            {
                Debug.Log("Send to MQTT");
               
                
                await managedMqttClient.EnqueueAsync(message);
                
            }

       
         

            private async void OnDestroy()
            {
                Debug.Log("Stopping MqttClient");
                await managedMqttClient.StopAsync();
            }
        }
    }
}