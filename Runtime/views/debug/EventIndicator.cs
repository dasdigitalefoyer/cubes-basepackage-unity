using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleCubes.Applications.PerformanceBot.Views {
	public class EventIndicator : MonoBehaviour {
		#region Inspector Fields
		[SerializeField]
		private float activeDuration = 1f;
		[SerializeField]
		private float fadeDuration = 0.3f;
		[SerializeField]
		private bool keepState = false;
		#endregion

		#region Private Fields
		private Image image;
		private Color color;
		private bool stateActive = false;
		#endregion

		#region MonoBehaviour Callbacks
		private void Awake() {
			image = GetComponent<Image>();
		}

		private void OnValidate() {
			if (!image) {
				image = GetComponent<Image>();
			}
			if (image) {
				Color color = image.color;
				color.a = 0.1f;
				image.color = color;
			}
		}
		#endregion

		#region Public Methods
		public void SetActive() {
			StopCoroutine(nameof(Activate));
			StartCoroutine(Activate());
		}

		public void SetActive(bool active) {
			activeDuration = 0;
			stateActive = active;
			StopCoroutine(nameof(Activate));
			StartCoroutine(Activate());
		}
		#endregion

		#region Private Methods
		private IEnumerator Activate() {
			float progressPerSecond = float.MaxValue;
			if (fadeDuration > 0) {
				progressPerSecond = 1f / fadeDuration;
			}
			color = image.color;
			while (!Fade(true, progressPerSecond)) {
				yield return null;
			}
			if (keepState && stateActive) {
				yield break;
			}
			yield return new WaitForSecondsRealtime(activeDuration);
			while (!Fade(false, progressPerSecond)) {
				yield return null;
			}
		}

		private bool Fade(bool fadingToActive, float progressPerSecond) {
			if (fadingToActive) {
				color.a += progressPerSecond * Time.deltaTime;
			} else {
				color.a -= progressPerSecond * Time.deltaTime;
			}
			color.a = Mathf.Clamp(color.a, 0.1f, 1f);
			image.color = color;
			if ((fadingToActive && color.a == 1) || (!fadingToActive && color.a == 0.1f)) {
				return true;
			}
			return false;
		}
		#endregion
	}
}
