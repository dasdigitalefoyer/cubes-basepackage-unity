using System.Collections.Generic;

using System.Linq.Expressions;
using System.Runtime.Serialization;
using System;

using UnityEngine;
namespace PuzzleCubes
{
   
    using Models;
    using MQTTnet;
    using UnityEngine.Events;
     using Newtonsoft.Json;

    namespace Communication
    {
        // [RequireComponent(typeof(CommunicationManager))]
        public class EventDispatcher : MonoBehaviour
        {
            public static string appStateTopic =  "puzzleCubes/app/state";
            // public static string appSubTopics = "puzzleCubes/app/+";
            public string dedicatedAppTopicPrefix(string cubeId) =>  $"puzzleCubes/{cubeId}/app/";

        

            protected IDictionary<Type, Action<EventDispatcher, object>> jsonTypeToEventMap 
                = new Dictionary<Type, Action<EventDispatcher, object>> (  )
            {
                { typeof(CubeControl), (x,o) => x.cubeControlEvent.Invoke(o as CubeControl)},
                { typeof(CubeState), (x,o) => x.cubeStateEvent.Invoke(o as CubeState)},
                { typeof(Notification), (x,o) => x.notificationEvent.Invoke(o as Notification)},
            };

            

            

            public CubeControlEvent cubeControlEvent;
            public CubeStateEvent cubeStateEvent;
            public NotificationEvent notificationEvent;

            public AppDatagramEvent appDatagramEvent;

            public MqttCommunication mqttCommunication;
            public ZmqCommunication zmqCommunication;

            public AppState appState;
            protected virtual void Initialize()
            {
               mqttCommunication.Subscribe("test", HandleTest );
            }

            protected void HandleTest(MqttApplicationMessage msg, IList<string> wildcardItem){
                Debug.Log("HandleTest: " + msg.Payload );
            }

            void Start()
            {
                if(mqttCommunication == null)
                    mqttCommunication = GameObject.FindObjectOfType<MqttCommunication>();
                if(zmqCommunication == null)
                    zmqCommunication = GameObject.FindObjectOfType<ZmqCommunication>();
          
                Initialize();
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
                    appDatagramEvent.Invoke(data.AppDatagram);
            }

       
            public void HandleJsonEvent(JsonDatagram data)
            {

                Debug.Log("HandleJsonEvent");
                // return;
                foreach(KeyValuePair<Type, Action<EventDispatcher, object>> t in jsonTypeToEventMap)
                {
                    
                    string className = t.Key.Name;
                    string jsonKey = char.ToLower(className[0]) + className.Substring(1);
                    // Debug.Log("checking:" + className + " -> " + jsonKey);
                    
                    if(data.TokenData != null && data.TokenData.ContainsKey(jsonKey))
                    {
                        Debug.Log("found:" + className + " -> " + jsonKey + " in TokenData");
                        try{
                            var o = data.TokenData[jsonKey].ToObject(t.Key);

                            if(o != null)
                                t.Value(this,o);
                        }
                        catch(Exception e)
                        {
                            Debug.LogError(e.Message);
                        }
                        

                    }

                }
            
            }
            // public void HandleMqttEvent(MqttApplicationMessage message)
            // {

            //     Debug.Log("HandleMqttEvent");
            // }

            public void DispatchAppStateEvent(AppState state)
            {
                Debug.Log("HandleStateEvent");
                appState = state;
                JsonDatagram jd = JsonDatagram.CreateFrom(state);
                 var json = JsonConvert.SerializeObject(jd, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                // this.SendZmq(json, true);  
                var msg = new MqttApplicationMessage();
                msg.Topic = appStateTopic;
                msg.Payload = System.Text.Encoding.UTF8.GetBytes(json);
                
                this.mqttCommunication.Send(msg); 
                
                
            }



            protected void SendZmq(string message, bool enqueue = false)
            {
                if(zmqCommunication == null)
                    zmqCommunication = GameObject.FindObjectOfType<ZmqCommunication>();
                if(zmqCommunication != null)
                {
                    zmqCommunication.Send(message, enqueue);
                }
            }


           
        }
    }
}