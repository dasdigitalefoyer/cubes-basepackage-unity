namespace PuzzleCubes.Controller
{
    using System;
    using System.Data;
    using PuzzleCubes.Communication;
    using PuzzleCubes.Models;
    using UnityEngine;
    using System.Threading.Tasks;
   
    using System.Collections;

    public class AppController : MonoBehaviour
    {
        public  AppState state = new AppState();
        public AppStateEvent stateEvent;

        public MqttCommunication mqttCommunication;



        void Awake()
        {
            state.CubeId = SystemInfo.deviceName;
            state.IsRunning = true;

            
        }

       

        protected virtual void Initialize()
        {

        }

        async void Start()
        {
            Initialize();
            if(mqttCommunication == null)
                mqttCommunication = GameObject.FindObjectOfType<MqttCommunication>();
         
            mqttCommunication.Initialize( state.CubeId);
            
            StartCoroutine(DispatchState());
            await Task.CompletedTask;

        }

        protected IEnumerator DispatchState()
        {
            yield return new WaitForEndOfFrame();
            if(stateEvent != null)
                stateEvent.Invoke(state);
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