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

		protected virtual void Initialize() { }

		void Awake() {
			state.AppVersion = Application.version;
			state.CubeId = SystemInfo.deviceName;
			state.ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
			state.IsRunning = true;
			Initialize();

			stateDirty = true;
		}

		void Start() {
			StartCoroutine(DispatchState());
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
