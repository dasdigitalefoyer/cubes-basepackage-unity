using PuzzleCubes.Models;
using UnityEngine;

namespace PuzzleCubes.Views {
	public class CubeControlDebugView : MonoBehaviour {
		#region Inspector Fields
		[Header("Event indicator references")]
		[SerializeField]
		private EventIndicator translationStepForward;
		[SerializeField]
		private EventIndicator translationStepBackward;
		[SerializeField]
		private EventIndicator translationStepLeft;
		[SerializeField]
		private EventIndicator translationStepRight;
		[SerializeField]
		private EventIndicator rotationStepLeft;
		[SerializeField]
		private EventIndicator rotationStepRight;
		[SerializeField]
		private EventIndicator rotationImpulseLeft;
		[SerializeField]
		private EventIndicator rotationImpulseRight;
		[SerializeField]
		private EventIndicator isMoving;


		[SerializeField]
		private Transform direction;

		[SerializeField]
		private EventIndicator tap;
		#endregion

		#region Private Fields
		#endregion

		#region MonoBehaviour Callbacks
		#endregion

		#region Public Methods
		public void HandleCubeControl(CubeControl cubeControl) {
			if (cubeControl == null) return;

			if (cubeControl.TranslationStepForward.GetValueOrDefault()) TranslationStepForward();
			if (cubeControl.TranslationStepBackward.GetValueOrDefault()) TranslationStepBackward();
			if (cubeControl.TranslationStepLeft.GetValueOrDefault()) TranslationStepLeft();
			if (cubeControl.TranslationStepRight.GetValueOrDefault()) TranslationStepRight();

			if (cubeControl.RotationImpulseLeft.GetValueOrDefault()) RotationImpulseLeft();
			if (cubeControl.RotationImpulseRight.GetValueOrDefault()) RotationImpulseRight();
			if (cubeControl.RotationStepLeft.GetValueOrDefault()) RotationStepLeft();
			if (cubeControl.RotationStepRight.GetValueOrDefault()) RotationStepRight();

			if (cubeControl.Moving.HasValue) {
				IsMoving((bool)cubeControl.Moving);
			}

			direction.rotation = Quaternion.Euler(0.0f, 0.0f, cubeControl.Orientation.GetValueOrDefault()); 

			if (cubeControl.Tap.GetValueOrDefault()) Tap();
		}

		public void TranslationStepForward() => translationStepForward?.SetActive();
		public void TranslationStepBackward() => translationStepBackward?.SetActive();
		public void TranslationStepLeft() => translationStepLeft?.SetActive();
		public void TranslationStepRight() => translationStepRight?.SetActive();
		public void RotationStepLeft() => rotationStepLeft?.SetActive();
		public void RotationStepRight() => rotationStepRight?.SetActive();
		public void RotationImpulseLeft() => rotationImpulseLeft?.SetActive();
		public void RotationImpulseRight() => rotationImpulseRight?.SetActive();
		public void IsMoving(bool moving) => isMoving?.SetActive(moving);
		public void Tap() => tap?.SetActive();
		#endregion

		#region Private Methods
		#endregion
	}
}
