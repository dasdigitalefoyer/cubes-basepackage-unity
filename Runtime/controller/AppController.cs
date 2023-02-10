namespace PuzzleCubes.Controller
{
    using System;
    using System.Data;
    using PuzzleCubes.Communication;
    using PuzzleCubes.Models;
    using UnityEngine;
    using System.Threading.Tasks;
    class AppController: MonoBehaviour
    {
        public Models.AppState state;
        public AppStateEvent stateEvent;

        public MqttCommunication mqttCommunication;



        

        void Awake()
        {
            state.CubeId = SystemInfo.deviceName;
           

            if(stateEvent != null)
                stateEvent.Invoke(state);
        }

        async void Start()
        {
            if(mqttCommunication == null)
                mqttCommunication = GameObject.FindObjectOfType<MqttCommunication>();
            mqttCommunication.Initialize( state.CubeId);
            await Task.CompletedTask;
        }
    }

   
    
}