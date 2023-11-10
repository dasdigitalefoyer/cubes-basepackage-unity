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
    using MQTTnet.Packets;
    using PuzzleCubes.Controller;

    namespace Communication
    {
        // [RequireComponent(typeof(CommunicationManager))]
        public class EventDispatcher : MonoBehaviour
        {
            // public static string appStateTopic =  "puzzleCubes/app/state";
            public string appStateTopic(string cubeId) =>  $"puzzleCubes/{cubeId}/app/state";
            public static string debugTopic = "puzzleCubes/debug";
            // public static string appSubTopics = "puzzleCubes/app/+";
            public string dedicatedAppTopicPrefix(string cubeId) =>  $"puzzleCubes/{cubeId}/app/";

            public string validConnectionTopic(string cubeId) =>  $"puzzleCubes/{cubeId}/app/connection";

            public const string posesTopic = "puzzleCubes/poses";
        
            protected IDictionary<MqttTopicFilter, MqttActions.Message> subscriptions 
                = new Dictionary<MqttTopicFilter, MqttActions.Message> (  );

            // mapping of backend events to unity Events
            protected IDictionary<Type, Action<EventDispatcher, object>> jsonTypeToEventMap 
                = new Dictionary<Type, Action<EventDispatcher, object>> (  )
                {
                    { typeof(CubeControl), (x,o) => x.cubeControlEvent.Invoke(o as CubeControl)},
                    { typeof(CubeState), (x,o) => x.cubeStateEvent.Invoke(o as CubeState)},
                    { typeof(Notification), (x,o) => x.notificationEvent.Invoke(o as Notification)},                    
                    { typeof(DebugControl), (x,o) => x.debugControlEvent.Invoke(o as DebugControl)},
                };

            

            

            public CubeControlEvent cubeControlEvent;
            public CubeStateEvent cubeStateEvent;
            public NotificationEvent notificationEvent;
            public ValidConnectionEvent validConnectionEvent;
            public DebugControlEvent debugControlEvent;

            public AppDatagramEvent appDatagramEvent;

            public CubePoseEvent poseEvent;

            public MqttCommunication mqttCommunication;
            public ZmqCommunication zmqCommunication;
            public AppController appController;

            protected AppState appState;

            protected virtual void Initialize()
            {
                // add MQTT subscriptions
                // subscriptions.Add(new MqttTopicFilterBuilder().WithTopic("test").WithNoLocal().Build(), HandleTest);
                subscriptions.Add(new MqttTopicFilterBuilder().WithTopic(validConnectionTopic(appController.state.CubeId)).Build(), HandleValidConnection);
                subscriptions.Add(new MqttTopicFilterBuilder().WithTopic(debugTopic).WithNoLocal().Build(), HandleDebugControl);
                subscriptions.Add(new MqttTopicFilterBuilder().WithTopic(posesTopic).WithNoLocal().Build(), HandlePoses);

                // create last will message with running = false
                AppState s = new AppState();
                s.AppName = appController.state.AppName;
                s.CubeId = appController.state.CubeId;


                var json = JsonConvert.SerializeObject(s, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.Objects
                });
                var msg = new MqttApplicationMessage();
                msg.Topic = appStateTopic(s.CubeId);
                msg.Payload = System.Text.Encoding.UTF8.GetBytes(json);
                msg.Retain = true;
                mqttCommunication.Initialize(appController.state.CubeId + "." + appController.state.AppName, msg);

            }

           

            protected virtual void PostInitialize()
            {
          

                mqttCommunication.Subscribe(subscriptions);
            }

            private void HandlePoses(MqttApplicationMessage msg, IList<string> wildcardItem)
            {
                var data = System.Text.Encoding.UTF8.GetString(msg.Payload);
                var result = JsonConvert.DeserializeObject<CubePose[]>(data);
                foreach(var dc in result)
                {
                    
                    poseEvent?.Invoke(dc);
                }
                
            }

            // protected void HandleTest(MqttApplicationMessage msg, IList<string> wildcardItem){
            //     Debug.Log("HandleTest: " + msg.Payload );
            // }
            
            // From MQTT
            protected void HandleDebugControl(MqttApplicationMessage msg, IList<string> wildcardItem)
            {
                var data = System.Text.Encoding.UTF8.GetString(msg.Payload);
                var result = JsonConvert.DeserializeObject<DebugControl>(data);
                if (result != null)
                    debugControlEvent?.Invoke(result);
            }

            // From MQTT
            private void HandleValidConnection(MqttApplicationMessage msg, IList<string> wildcarditems)
            {
                var data = System.Text.Encoding.UTF8.GetString(msg.Payload);
                var result = JsonConvert.DeserializeObject<ValidConnection>(data);
                if (result != null)
                    validConnectionEvent.Invoke(result);
            }

            void Start()
            {
                if(mqttCommunication == null)
                    mqttCommunication = this.GetComponentInChildren <MqttCommunication>();
                if(zmqCommunication == null)
                    zmqCommunication = this.GetComponentInChildren<ZmqCommunication>();
                if(appController == null)
                    appController = this.GetComponentInChildren<AppController>();
          
                Initialize();
                PostInitialize();
            }

      

       
            // EVENTS FROM CUBE BACKEND (e.g. CubeControl)
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
      

            public void DispatchAppStateEvent(AppState state)
            {
                Debug.Log("HandleStateEvent");
                // appState = state;
                // JsonDatagram jd = JsonDatagram.CreateFrom(state);
                var json = JsonConvert.SerializeObject(state, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.Objects
                });
                // this.SendZmq(json, true);  
                var msg = new MqttApplicationMessage();
                msg.Topic = appStateTopic(appController.state.CubeId);
                msg.Payload = System.Text.Encoding.UTF8.GetBytes(json);
                msg.MessageExpiryInterval = 3600;
                msg.Retain = true;
                
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