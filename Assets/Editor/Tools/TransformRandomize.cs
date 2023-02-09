using System;
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
		private static bool rotationX;
		private static bool rotationY;
		private static bool rotationZ;

		private static Transform[] selectedTransforms;
		private static GUIStyle labelGuiStyle;
		private static EditorWindow windowInstance;
		private static int selectedTransformCount;

		[MenuItem("Horang/Tools/Transform Randomize")]
		public static void ShowEditorWindow()
		{
			windowInstance = GetWindow(typeof(TransformRandomize), true, "Transform Randomize", true);
			windowInstance.minSize = new Vector2(350.0f, 400.0f);
			windowInstance.maxSize = new Vector2(350.0f, 400.0f);
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
			windowInstance.minSize = new Vector2(350.0f, 510.0f);
			windowInstance.maxSize = new Vector2(350.0f, 510.0f);
		}

		private void OnSelectionChange()
		{
			selectedTransformCount = Selection.count;
			selectedTransforms = Selection.transforms;
			
			windowInstance.Focus();
		}

		private void OnGUI()
		{
			EditorGUILayout.Space(20.0f);
			EditorGUILayout.LabelField("<color=#9F83FA><size=25><b>~ Transform Randomize ~</b></size></color>", labelGuiStyle);
			
			EditorGUILayout.Space(30.0f); // title top margin
			EditorGUILayout.LabelField($"<color=#7C76E8><size=15><b>Selected transform count = {selectedTransformCount}</b></size></color>", labelGuiStyle);
			EditorGUILayout.Separator();
			EditorGUILayout.Space(10.0f); // title bottom margin

			if (selectedTransformCount < 1 || selectedTransforms.Length < 1)
			{
				EditorGUILayout.LabelField($"<color=#C777FD><size=15><b>Select game object 1 or more.</b></size></color>", labelGuiStyle);
				
				return;
			}
			
			// scale
			showScale = EditorGUILayout.BeginFoldoutHeaderGroup(showScale, "Randomize Scale");
			if (showScale)
			{
				EditorGUI.indentLevel++;
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
						foreach (var transform in selectedTransforms)
						{
							var scale = Random.Range(minimumEqualScale, maximumEqualScale);

							transform.localScale = Vector3.one * scale;
						}
					}
					else
					{
						foreach (var transform in selectedTransforms)
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
					foreach (var transform in selectedTransforms)
					{
						transform.localScale = Vector3.one;
					}
				}
			}
			EditorGUI.indentLevel--;
			EditorGUILayout.EndFoldoutHeaderGroup();
			
			EditorGUILayout.Space(25.0f);
			
			// rotation
			showRotation = EditorGUILayout.BeginFoldoutHeaderGroup(showRotation, "Randomize Rotation");
			if (showRotation)
			{
				var style = GUI.skin.button;
				style.margin.left = 20;
				style.margin.right = 20;
				
				EditorGUI.indentLevel++;
				rotationX = EditorGUILayout.Toggle("Rotate x axis", rotationX);
				rotationY = EditorGUILayout.Toggle("Rotate y axis", rotationY);
				rotationZ = EditorGUILayout.Toggle("Rotate z axis", rotationZ);
				
				EditorGUILayout.Space(10.0f);
				
				var originalBackgroundColor = GUI.backgroundColor;
				GUI.backgroundColor = Color.HSVToRGB(232.0f / 360.0f, 53.0f / 100.0f, 99.0f / 100.0f);
				
				if (GUILayout.Button("Apply", GUILayout.Height(30.0f)))
				{
					foreach (var transform in selectedTransforms)
					{
						var x = rotationX ? Random.Range(0.0f, 360.0f) : transform.localRotation.eulerAngles.x;
						var y = rotationY ? Random.Range(0.0f, 360.0f) : transform.localRotation.eulerAngles.y;
						var z = rotationZ ? Random.Range(0.0f, 360.0f) : transform.localRotation.eulerAngles.z;
					
						transform.localRotation = Quaternion.Euler(x, y, z);
					}
				}

				GUI.backgroundColor = originalBackgroundColor;
				
				if (GUILayout.Button("Reset", GUILayout.Height(30.0f)))
				{
					foreach (var transform in selectedTransforms)
					{
						transform.localRotation = Quaternion.identity;
					}
				}
			}
			EditorGUI.indentLevel--;
			EditorGUILayout.EndFoldoutHeaderGroup();
		}
	}
}