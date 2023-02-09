using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Editor.Tools
{
	public sealed class TransformRandomize : EditorWindow
	{
		// scale
		private static bool showScale = true;
		private static bool useEqualScale;
		private static Vector3 minimumScale;
		private static Vector3 maximumScale;
		private static float minimumEqualScale;
		private static float maximumEqualScale;
		
		// rotation
		private static bool showRotation = true;
		private static bool lockXAxis;
		private static bool lockYAxis;
		private static bool lockZAxis;
		
		// position
		private static bool showPosition = true;
		private static float moveRange;
		private static bool lockXPosition;
		private static bool lockYPosition;
		private static bool lockZPosition;

		private static Transform[] selectedTransforms;
		private static GUIStyle labelGuiStyle;
		private static EditorWindow windowInstance;

		[MenuItem("Horang/Tools/Transform Randomize")]
		public static void ShowEditorWindow()
		{
			windowInstance = GetWindow(typeof(TransformRandomize), true, "Transform Randomize", true);
			windowInstance.minSize = new Vector2(350.0f, 760.0f);
			windowInstance.maxSize = new Vector2(350.0f, 760.0f);
		}

		private void OnEnable()
		{
			labelGuiStyle = new GUIStyle
			{
				richText = true,
				alignment = TextAnchor.MiddleCenter,
			};
		}

		private void OnFocus()
		{
			windowInstance = GetWindow(typeof(TransformRandomize), true, "Transform Randomize", true);
			windowInstance.minSize = new Vector2(350.0f, 760.0f);
			windowInstance.maxSize = new Vector2(350.0f, 760.0f);
		}

		private void OnSelectionChange()
		{
			selectedTransforms = Selection.transforms;
			
			windowInstance.Focus();
		}

		private void OnGUI()
		{
			var count = selectedTransforms?.Length ?? 0;
			
			EditorGUILayout.Space(20.0f);
			EditorGUILayout.LabelField("<color=#9F83FA><size=25><b>Transform Randomize</b></size></color>", labelGuiStyle);
			EditorGUILayout.Space(30.0f);
			
			EditorGUILayout.LabelField($"<color=#7C76E8><size=15>Selected transform count = {count}</size></color>", labelGuiStyle);
			EditorGUILayout.Space(10.0f);

			if (count < 1)
			{
				EditorGUILayout.LabelField($"<color=#FF5577><size=20><b>Select game object 1 or more.</b></size></color>", labelGuiStyle);
				
				return;
			}
			
			// position
			showPosition = EditorGUILayout.BeginFoldoutHeaderGroup(showPosition, "Randomize Position");
			EditorGUI.indentLevel++;
			if (showPosition)
			{
				EditorGUILayout.HelpBox("Can move by given range. (Work accumulate current position)", MessageType.None);
				
				var style = GUI.skin.button;
				style.margin.left = 20;
				style.margin.right = 20;
				
				moveRange = EditorGUILayout.FloatField("Move range", moveRange);
				lockXPosition = EditorGUILayout.Toggle("Lock x axis", lockXPosition);
				lockYPosition = EditorGUILayout.Toggle("Lock y axis", lockYPosition);
				lockZPosition = EditorGUILayout.Toggle("Lock z axis", lockZPosition);
				
				EditorGUILayout.Space(10.0f);
				
				var originalBackgroundColor = GUI.backgroundColor;
				GUI.backgroundColor = Color.HSVToRGB(232.0f / 360.0f, 53.0f / 100.0f, 99.0f / 100.0f);
				
				if (GUILayout.Button("Apply", GUILayout.Height(30.0f)))
				{
					foreach (var transform in selectedTransforms!)
					{
						var x = lockXPosition is false ? transform.localPosition.x + Random.Range(-moveRange, moveRange) : transform.localPosition.x;
						var y = lockYPosition is false ? transform.localPosition.y + Random.Range(-moveRange, moveRange) : transform.localPosition.y;
						var z = lockZPosition is false ? transform.localPosition.z + Random.Range(-moveRange, moveRange) : transform.localPosition.z;
					
						transform.localPosition = new Vector3(x, y, z);
					}
				}

				GUI.backgroundColor = originalBackgroundColor;
				
				if (GUILayout.Button("Reset", GUILayout.Height(30.0f)))
				{
					foreach (var transform in selectedTransforms!)
					{
						transform.localPosition = Vector3.zero;
					}
				}
			}
			EditorGUI.indentLevel--;
			EditorGUILayout.EndFoldoutHeaderGroup();
			
			EditorGUILayout.Space(25.0f);
			
			// rotation
			showRotation = EditorGUILayout.BeginFoldoutHeaderGroup(showRotation, "Randomize Rotation");
			EditorGUI.indentLevel++;
			if (showRotation)
			{
				EditorGUILayout.HelpBox("Can rotate by selected axis.", MessageType.None);
				
				var style = GUI.skin.button;
				style.margin.left = 20;
				style.margin.right = 20;
				
				lockXAxis = EditorGUILayout.Toggle("Lock x axis", lockXAxis);
				lockYAxis = EditorGUILayout.Toggle("Lock y axis", lockYAxis);
				lockZAxis = EditorGUILayout.Toggle("Lock z axis", lockZAxis);
				
				EditorGUILayout.Space(10.0f);
				
				var originalBackgroundColor = GUI.backgroundColor;
				GUI.backgroundColor = Color.HSVToRGB(232.0f / 360.0f, 53.0f / 100.0f, 99.0f / 100.0f);
				
				if (GUILayout.Button("Apply", GUILayout.Height(30.0f)))
				{
					foreach (var transform in selectedTransforms!)
					{
						var x = lockXAxis is false ? Random.Range(0.0f, 360.0f) : transform.localRotation.eulerAngles.x;
						var y = lockYAxis is false ? Random.Range(0.0f, 360.0f) : transform.localRotation.eulerAngles.y;
						var z = lockZAxis is false ? Random.Range(0.0f, 360.0f) : transform.localRotation.eulerAngles.z;
					
						transform.localRotation = Quaternion.Euler(x, y, z);
					}
				}

				GUI.backgroundColor = originalBackgroundColor;
				
				if (GUILayout.Button("Reset", GUILayout.Height(30.0f)))
				{
					foreach (var transform in selectedTransforms!)
					{
						transform.localRotation = Quaternion.identity;
					}
				}
			}
			EditorGUI.indentLevel--;
			EditorGUILayout.EndFoldoutHeaderGroup();
			
			EditorGUILayout.Space(25.0f);

			// scale
			showScale = EditorGUILayout.BeginFoldoutHeaderGroup(showScale, "Randomize Scale");
			EditorGUI.indentLevel++;
			if (showScale)
			{
				EditorGUILayout.HelpBox("Can difference scale each axis, or equality scale on every axis.", MessageType.None);
				
				useEqualScale = EditorGUILayout.Toggle("Use equal scale", useEqualScale);
				
				var style = GUI.skin.textField;
				style.margin.right = 20;

				if (useEqualScale)
				{
					minimumEqualScale = EditorGUILayout.FloatField("Minimum equal scale", minimumEqualScale, style);
					maximumEqualScale = EditorGUILayout.FloatField("Maximum equal scale", maximumEqualScale, style);
				}
				else
				{
					minimumScale = EditorGUILayout.Vector3Field("Minimum scale", minimumScale);
					maximumScale = EditorGUILayout.Vector3Field("Maximum scale", maximumScale);
				}
				
				EditorGUILayout.Space(10.0f);
				
				var originalBackgroundColor = GUI.backgroundColor;
				GUI.backgroundColor = Color.HSVToRGB(232.0f / 360.0f, 53.0f / 100.0f, 99.0f / 100.0f);

				if (GUILayout.Button("Apply", GUILayout.Height(30.0f)))
				{
					if (useEqualScale)
					{
						foreach (var transform in selectedTransforms!)
						{
							var scale = Random.Range(minimumEqualScale, maximumEqualScale);

							transform.localScale = Vector3.one * scale;
						}
					}
					else
					{
						foreach (var transform in selectedTransforms!)
						{
							var x = Random.Range(minimumScale.x, maximumScale.x);
							var y = Random.Range(minimumScale.y, maximumScale.y);
							var z = Random.Range(minimumScale.z, maximumScale.z);

							transform.localScale = new Vector3(x, y, z);
						}
					}
				}
				
				GUI.backgroundColor = originalBackgroundColor;

				if (GUILayout.Button("Reset", GUILayout.Height(30.0f)))
				{
					foreach (var transform in selectedTransforms!)
					{
						transform.localScale = Vector3.one;
					}
				}
			}
			EditorGUI.indentLevel--;
			EditorGUILayout.EndFoldoutHeaderGroup();
		}
	}
}