namespace PuzzleCubes.Controller
{
    using System;
    using System.Data;
    using PuzzleCubes.Communication;
    using PuzzleCubes.Models;
    using UnityEngine;
    using System.Threading.Tasks;
   
    using System.Collections;
    using MQTTnet;
    using UnityEditor;

    public class AppController : MonoBehaviour
    {
        public  AppState state = new AppState();
        public AppStateEvent stateEvent;

        // public MqttCommunication mqttCommunication;

        protected bool stateDirty = false;



        void Awake()
        {
            state.CubeId = SystemInfo.deviceName;
          
            
        }

       

        protected virtual void Initialize()
        {

        }

        async void Start()
        {
            Initialize();

           
            state.IsRunning = true;
            stateDirty = true;

            StartCoroutine(DispatchState());
            await Task.CompletedTask;

        }

        protected IEnumerator DispatchState()
        {
            yield return new WaitForEndOfFrame();
            if(stateDirty && stateEvent != null)
                stateEvent.Invoke(state);
            stateDirty = false;
        }

        void OnApplicationQuit()
        {
            Debug.Log("AppController::OnApplicationQuit");
            state.IsRunning = false;
            if(stateEvent != null)
                stateEvent.Invoke(state);
        }



       
    }

   
    
}