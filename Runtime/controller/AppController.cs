using PuzzleCubes.Models;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;

namespace PuzzleCubes.Controller {
	public class AppController : MonoBehaviour {
		public AppState state = new AppState();
		public AppStateEvent stateEvent;
		// public MqttCommunication mqttCommunication;

		protected bool stateDirty = false;

		void Awake() {
			state.AppVersion = Application.version;
			state.CubeId = SystemInfo.deviceName;
		}

		protected virtual void Initialize() { }

		async void Start() {
			Initialize();

			state.ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
			state.IsRunning = true;
			stateDirty = true;

			StartCoroutine(DispatchState());
			await Task.CompletedTask;
		}

		protected IEnumerator DispatchState() {
			while (true) {
				yield return new WaitForEndOfFrame();
				if (stateDirty && stateEvent != null) {
					stateEvent.Invoke(state);
					stateDirty = false;
				}
			}
		}

		void OnApplicationQuit() {
			Debug.Log("AppController::OnApplicationQuit");
			state.IsRunning = false;
			if (stateEvent != null)
				stateEvent.Invoke(state);
		}

		public void HandleVolume(float volume) {
			state.Volume = volume;
			stateDirty = true;
		}

		public void HandleMqttConnected(bool connected) {
			state.MqttConnected = connected;
			stateDirty = true;
		}
	}
}
