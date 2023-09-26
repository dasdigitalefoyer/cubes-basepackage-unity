using UnityEngine;

/// <summary>
/// Mark a field to be read-only in the unity inspector
/// source: https://dev.to/jayjeckel/unity-tips-properties-and-the-inspector-1goo
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class ReadOnlyFieldAttribute : PropertyAttribute { }
