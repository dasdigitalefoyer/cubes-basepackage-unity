namespace PuzzleCubes.Controller
{
    using System;
    using System.Data;
    using PuzzleCubes.Communication;
    using PuzzleCubes.Models;
    using UnityEngine;
    using System.Threading.Tasks;
   
    using System.Collections;

    class AppController: MonoBehaviour
    {
        public Models.AppState state;
        public AppStateEvent stateEvent;

        public MqttCommunication mqttCommunication;



        void Awake()
        {
            state.CubeId = SystemInfo.deviceName;
            state.IsRunning = true;

            
        }

        async void Start()
        {
            if(mqttCommunication == null)
                mqttCommunication = GameObject.FindObjectOfType<MqttCommunication>();
         
            mqttCommunication.Initialize( state.CubeId);
            
            StartCoroutine(DispatchState());
            await Task.CompletedTask;

        }

        IEnumerator DispatchState()
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