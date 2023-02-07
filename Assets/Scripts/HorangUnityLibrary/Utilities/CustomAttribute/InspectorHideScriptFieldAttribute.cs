using System;
using UnityEditor;

namespace Horang.HorangUnityLibrary.Utilities.CustomAttribute
{
	[CustomEditor(typeof(object), true)]
	public sealed class InspectorHideScriptFieldEditor : Editor
	{
		private bool hideScriptField;

		private void OnEnable()
		{
			hideScriptField = target.GetType().GetCustomAttributes(typeof(InspectorHideScriptField), false).Length > 0;
		}

		public override void OnInspectorGUI()
		{
			if (hideScriptField)
			{
				serializedObject.Update();
				
				EditorGUI.BeginChangeCheck();
				
				DrawPropertiesExcluding(serializedObject, "m_Script");
				
				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
				}
			}
			else
			{
				base.OnInspectorGUI();
			}
		}
	}
	
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class InspectorHideScriptField : Attribute
	{
	}
}