using PuzzleCubes.Models;
using UnityEngine;

namespace PuzzleCubes.Controller {
	public class DebugController : MonoBehaviour {
		#region Inspector Fields
		[Header("References")]
		[SerializeField]
		private GameObject debugView;
		[SerializeField]
		private GameObject graphy;

		public bool ShowCubeOverlay {
			get => _showCubeOverlay;
			set {
				_showCubeOverlay = value;
				debugView.SetActive(_showCubeOverlay);
			}
		}
		public bool ShowGraphy {
			get => _showGraphy;
			set {
				_showGraphy = value;
				graphy.SetActive(_showGraphy);
			}
		}
		public bool ShowIngameDebugConsole {
			get => _showLogs;
			set {
				_showLogs = value;
				if (_showLogs) {
					IngameDebugConsole.DebugLogManager.Instance.ShowLogWindow();
				} else {
					IngameDebugConsole.DebugLogManager.Instance.HideLogWindow();
				}
			}
		}
		#endregion

		#region Private Fields
		[Header("Property backing fields (read-only)")]
		[SerializeField][ReadOnlyField]
		private bool _showCubeOverlay;
		[SerializeField][ReadOnlyField]
		private bool _showGraphy;
		[SerializeField][ReadOnlyField]
		private bool _showLogs;
        #endregion

        #region MonoBehaviour Callbacks
#if !UNITY_EDITOR
		private void Awake() {
			ShowCubeOverlay = false;
			ShowGraphy = false;
			ShowLogs = false;
		}
#endif

        private void OnValidate() {
            if (!debugView) {
				Debug.Log("DebugController missing reference to debugView");
            }
			if (!debugView) {
				Debug.Log("DebugController missing reference to graphy");
			}
		}
        #endregion

        #region Public Methods
        public void OnNotificationEvent(Notification notification) {
			switch (notification.Type) {
				case Notification.TypeEnum.INFO:
					Debug.Log(notification.Message);
					break;
				case Notification.TypeEnum.DEBUG:
					Debug.Log(notification.Message);
					break;
				case Notification.TypeEnum.WARNING:
					Debug.LogWarning(notification.Message);
					break;
				case Notification.TypeEnum.ERROR:
					Debug.LogError(notification.Message);
					break;
				default:
					break;
			}
		}
		#endregion
	}
}
