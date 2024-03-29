namespace PuzzleCubes.Controller
{
    using System;
    using Newtonsoft.Json.Linq;
    using PuzzleCubes.Communication;
    using PuzzleCubes.Models;
    using UnityEngine;
  
   
    using System.Collections.Generic;
    
    using UnityEditor;
    using UnityEngine.Animations;
    using Newtonsoft.Json;
    using MQTTnet;


    public class KeyboardController : MonoBehaviour
    {
        
        public JsonEvent jsonEvent;


        public float orientation;
        
        protected IDictionary<KeyCode, Action> keyToEventMap 
            = new Dictionary<KeyCode, Action> (  );

        protected IDictionary<String, Action<float>> axisToEventMap 
            = new Dictionary<String, Action<float>> (  );
       
        protected void dispatchObject(object o) {
			JsonDatagram jd = new JsonDatagram();
			jd.TokenData = new Dictionary<string, JToken>();
			string className = o.GetType().Name;
			string jsonKey = char.ToLower(className[0]) + className.Substring(1);

			JToken t = JToken.FromObject(o);
			
			jd.TokenData.Add(jsonKey, t);
			jsonEvent.Invoke(jd);
		}


          protected void dispatchAsMqtt(object o, String topic, MqttEvent e) {
            var json = JsonConvert.SerializeObject(o, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects
            });
             var msg = new MqttApplicationMessage();
            msg.Topic = topic;
            msg.Payload = System.Text.Encoding.UTF8.GetBytes(json);
            msg.MessageExpiryInterval = 3600;
            e.Invoke(msg, null);

			
		}

        
        protected virtual void Initialize()
        {
             
            // SETUP CUBECONTROL - BEGIN
                
            /* FORWARD */
            keyToEventMap.Add(KeyCode.W, () => {
                dispatchObject(new CubeControl {
					TranslationStepForward = true,
					Moving = true});
            } );
            /* BACKWARD */
            keyToEventMap.Add(KeyCode.S, () => {
                dispatchObject(new CubeControl {
					TranslationStepBackward = true,
					Moving = true});
            } );
            /* LEFT */
            keyToEventMap.Add(KeyCode.A, () => {
                dispatchObject(new CubeControl {
					TranslationStepLeft = true,
					Moving = true});
            } );
            /* RIGHT */
            keyToEventMap.Add(KeyCode.D, () => {
                dispatchObject(new CubeControl {
					TranslationStepRight = true,
					Moving = true});
            } );
            /* RSTEPLEFT */
            keyToEventMap.Add(KeyCode.Q, () => {
                dispatchObject(new CubeControl {
					RotationStepLeft = true,
					Moving = true});
            } );
            /* RSTEPRIGHT */
            keyToEventMap.Add(KeyCode.E, () => {
                dispatchObject(new CubeControl {
					RotationStepRight = true,
					Moving = true});
            } );
            /* NOMOVING */
            keyToEventMap.Add(KeyCode.Y, () => {
                dispatchObject(new CubeControl {Moving = false} ); } );
            
            /* NOMOVING */
            keyToEventMap.Add(KeyCode.G, () => {
                dispatchObject(new CubeControl  {				
					Tap = true});
            } );

            /* ORIENTATION */

            axisToEventMap.Add("Horizontal", (value) => {
                // Debug.Log("Horizontal: " + value);
                orientation-=value;
                //  orientation between -180 and 180 degrees
                if (orientation > 180) orientation -= 360;
                else if (orientation < -180) orientation += 360;


                dispatchObject(new CubeControl  {
                    Orientation =orientation});
            } );
           
            /* ORIENTATION - END */

            // SETUP CUBECONTROL - END

            // Toggle DebugView & Graphy
            keyToEventMap.Add(KeyCode.C, () => {
                DebugController dc = FindObjectOfType<DebugController>();
                if (dc) {
                    dc.ShowCubeOverlay = !dc.ShowCubeOverlay;
                    dc.ShowGraphy = !dc.ShowGraphy;
                }
            });
            // Toggle IngameDebugConsole
            keyToEventMap.Add(KeyCode.V, () => {
                DebugController dc = FindObjectOfType<DebugController>();
                if (dc) {
                    dc.ShowIngameDebugConsole = !dc.ShowIngameDebugConsole;
                }
            });
        }

        void Start()
        {
            this.Initialize();
        }

        void Update()
        {
            foreach(KeyValuePair<KeyCode, Action> kvp in keyToEventMap)
            {
                if(Input.GetKeyDown(kvp.Key)) kvp.Value();
            }
            foreach(KeyValuePair<String, Action<float>> kvp in axisToEventMap)
            {
                if( Input.GetAxis(kvp.Key) != 0)
                    kvp.Value( Input.GetAxis(kvp.Key));
            }
        }
    }
}