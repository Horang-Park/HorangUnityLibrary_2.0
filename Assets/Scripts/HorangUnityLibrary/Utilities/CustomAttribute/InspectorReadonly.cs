using System;
using UnityEditor;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Utilities.CustomAttribute
{
#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(InspectorReadonly))]
	public sealed class InspectorReadonlyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, true);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			GUI.enabled = false;

			switch (property.propertyType)
			{
				case SerializedPropertyType.Integer:
					EditorGUI.LabelField(position, label.text, property.intValue.ToString());
					break;
				case SerializedPropertyType.Boolean:
					EditorGUI.LabelField(position, label.text, property.boolValue.ToString());
					break;
				case SerializedPropertyType.Float:
					EditorGUI.LabelField(position, label.text, property.floatValue.ToString("F6"));
					break;
				case SerializedPropertyType.String:
					EditorGUI.LabelField(position, label.text, property.stringValue);
					break;
				case SerializedPropertyType.Color:
				case SerializedPropertyType.ObjectReference:
				case SerializedPropertyType.LayerMask:
				case SerializedPropertyType.Enum:
				case SerializedPropertyType.Vector2:
				case SerializedPropertyType.Vector3:
				case SerializedPropertyType.Vector4:
				case SerializedPropertyType.Rect:
				case SerializedPropertyType.ArraySize:
				case SerializedPropertyType.Character:
				case SerializedPropertyType.AnimationCurve:
				case SerializedPropertyType.Bounds:
				case SerializedPropertyType.Gradient:
				case SerializedPropertyType.Quaternion:
				case SerializedPropertyType.ExposedReference:
				case SerializedPropertyType.FixedBufferSize:
				case SerializedPropertyType.Vector2Int:
				case SerializedPropertyType.Vector3Int:
				case SerializedPropertyType.RectInt:
				case SerializedPropertyType.BoundsInt:
				case SerializedPropertyType.ManagedReference:
				case SerializedPropertyType.Hash128:
				case SerializedPropertyType.Generic:
				default:
					EditorGUI.PropertyField(position, property, label, true);
					break;
			}
			
			GUI.enabled = true;
		}
	}
#endif
	
	/// <summary>
	/// Show read-only property on inspector.
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public sealed class InspectorReadonly : PropertyAttribute
	{
	}
}