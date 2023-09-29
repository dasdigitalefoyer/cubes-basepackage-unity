namespace PuzzleCubes {
	namespace Models {
		using System;
		using UnityEngine;

		[Serializable]
		public class DebugControl : BaseData {
			public enum DebugControlType {
				None, UI, Console
			}

			[SerializeField] private DebugControlType? type = DebugControlType.None;
			[SerializeField] private bool? active = false;

			public DebugControlType? Type { get => type; set => type = value; }
			public bool? Active { get => active; set => active = value; }
		}
	}
}