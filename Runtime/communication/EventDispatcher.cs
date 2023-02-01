using System.Collections.Generic;

using System.Linq.Expressions;
using System.Runtime.Serialization;
using System;

using UnityEngine;
namespace PuzzleCubes
{
   
    using Models;

    using UnityEngine.Events;

    namespace Communication
    {
        // [RequireComponent(typeof(CommunicationManager))]
        public class EventDispatcher : MonoBehaviour
        {


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

            protected virtual void Initialize()
            {
                
            }

            void Start()
            {
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
                    if(data.TokenData.ContainsKey(jsonKey))
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
        }
    }
}