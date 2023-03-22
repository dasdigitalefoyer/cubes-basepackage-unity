using UnityEngine;
using TMPro;
using PuzzleCubes.Models;

namespace PuzzleCubes.Views {
	public class AppStateDebugView : MonoBehaviour {
		#region Inspector Fields
		[SerializeField]
		private TextMeshProUGUI textField;
		#endregion

		#region Private Fields
		#endregion

		#region MonoBehaviour Callbacks
		#endregion

		#region Public Methods
		public void AppStateEvent(AppState appState) {
			if (textField) {
				textField.SetText(appState.ToString());
			}
		}
		#endregion

		#region Private Methods
		#endregion
	}
}
