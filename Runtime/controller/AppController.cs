using PuzzleCubes.Models;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;

namespace PuzzleCubes.Controller {
	public class AppController : MonoBehaviour {
		public static AppController Instance { get; protected set; }
		public GameObject splashScreen;

		public CubePoseEvent ownPoseEvent;

		public AppState state = new AppState();
		public AppStateEvent stateEvent;
		// public MqttCommunication mqttCommunication;

		protected bool stateDirty = false;

		protected virtual void Initialize() { }

		void Awake() {
			Instance = this;
            if (!splashScreen) {
				splashScreen = GameObject.Find("SplashScreen");
            }

			state.AppVersion = Application.version;
			if(state.CubeId.Equals(string.Empty))
				state.CubeId = SystemInfo.deviceName;
			state.ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
			state.IsRunning = true;
			Initialize();

			stateDirty = true;
		}

		void Start() {
			StartCoroutine(DispatchState());

#if !UNITY_EDITOR
			Cursor.visible = false;
#endif
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


		public void HandleCubePose(Models.CubePose pose) {
			if(pose.CubeId == state.CubeId)
				ownPoseEvent?.Invoke(pose);
		}

		public void ShowSplashScreen(bool active) {
            if (splashScreen) {
				splashScreen.SetActive(active);
            }
        }

		public void ToggleSplashScreen() {
            if (splashScreen) {
				ShowSplashScreen(!splashScreen.activeSelf);
			}
        }
	}
}
