namespace PuzzleCubes.Controller
{
    using System;
    using Newtonsoft.Json.Linq;
    using PuzzleCubes.Communication;
    using PuzzleCubes.Models;
    using UnityEngine;
  
   
    using System.Collections.Generic;
    
    using UnityEditor;

    public class KeyboardController : MonoBehaviour
    {
        
        public JsonEvent jsonEvent;
        
        protected IDictionary<KeyCode, Action> keyToEventMap 
            = new Dictionary<KeyCode, Action> (  );
       
        protected void dispatchObject(object o) {
			JsonDatagram jd = new JsonDatagram();
			jd.TokenData = new Dictionary<string, JToken>();
			string className = o.GetType().Name;
			string jsonKey = char.ToLower(className[0]) + className.Substring(1);

			JToken t = JToken.FromObject(o);
			
			jd.TokenData.Add(jsonKey, t);
			jsonEvent.Invoke(jd);
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
                dispatchObject(new CubeControl () ); } );
            
            /* NOMOVING */
            keyToEventMap.Add(KeyCode.G, () => {
                dispatchObject(new CubeControl  {				
					Tap = true});
            } ); 

            // SETUP CUBECONTROL - END
          

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
        }
    }
}