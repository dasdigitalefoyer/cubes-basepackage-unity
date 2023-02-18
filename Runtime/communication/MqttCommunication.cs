

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
            
            MqttApplicationMessage lastWill;



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
                 await managedMqttClient.SubscribeAsync(topicFilter.Keys);
                 foreach(var kvp in topicFilter)
                    this.subscriptions.Add(kvp.Key.Topic,kvp.Value);
            }
            public async Task Connect()
            {

           
                {
                   
                    //  add last will -> state with disconnected:
                    
                    var    mqttClientOptions = new MqttClientOptionsBuilder()
                        .WithTcpServer(host, port)
                        .WithClientId(clientId)
                        .Build();
                    
                    if(lastWill != null)
                    {
                        mqttClientOptions.WillTopic = lastWill.Topic;
                        mqttClientOptions.WillPayload = lastWill.Payload;
                        mqttClientOptions.WillQualityOfServiceLevel = lastWill.QualityOfServiceLevel;
                        mqttClientOptions.WillRetain = lastWill.Retain;
                    }
                        


                    var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                        .WithClientOptions(mqttClientOptions)
                        .Build();

                    await managedMqttClient.StartAsync(managedMqttClientOptions);
                    
                    managedMqttClient.ApplicationMessageReceivedAsync +=  async (  args) => 
                    {

                        pendingMqttMessages.Enqueue(args.ApplicationMessage);
                       
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

            public async void Initialize(string id, MqttApplicationMessage lastWill = null)
            {
                this.clientId = id;
                this.lastWill = lastWill;
                
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


                    while(pendingMqttMessages.TryDequeue(out var message))
                    {
                        foreach(var kvp in subscriptions)
                        {
                            if(MqttTopicFilterComparer.Compare(message.Topic,kvp.Key) == MqttTopicFilterCompareResult.IsMatch)
                            {
                                kvp.Value( message, null);
                            }
                            
                        }
                
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
                float maxTimeout = 3;
                float time = Time.time;
                while (managedMqttClient.PendingApplicationMessagesCount > 0 && maxTimeout > Time.time -time)
                {
                     Debug.Log("Waiting for pending messages");
                    await Task.Delay(500);
                }
                await managedMqttClient.StopAsync();
            }
        }
    }
}