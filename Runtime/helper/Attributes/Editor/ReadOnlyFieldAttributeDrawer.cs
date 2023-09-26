using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Make a property read-only in the unity inspector
/// source: https://dev.to/jayjeckel/unity-tips-properties-and-the-inspector-1goo
/// </summary>
[UsedImplicitly, CustomPropertyDrawer(typeof(ReadOnlyFieldAttribute))]
public class ReadOnlyFieldAttributeDrawer : PropertyDrawer {
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	=> EditorGUI.GetPropertyHeight(property, label, true);

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		GUI.enabled = false;
		EditorGUI.PropertyField(position, property, label, true);
		GUI.enabled = true;
	}
}
