using System;
using System.Collections.Generic;
using System.IO;
using Horang.HorangUnityLibrary.Utilities;
using Horang.HorangUnityLibrary.Utilities.CustomAttribute;
using UnityEditor;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Modules.AudioModule
{
	[Serializable]
	internal struct AudioDataType
	{
		public enum AudioPlayType
		{
			None,
			BGM,
			OneshotSfx,
			LoopSfx
		}

		[InspectorReadonly] public string name;
		public AudioClip audioClip;

		[Header("Audio Clip Settings")]
		public AudioPlayType audioPlayType;
		[Range(0.0f, 1.0f)] public float audioClipVolume;
		[Range(-1.0f, 1.0f)] public float audioClipPan;

		[Header("Audio Clip Inspector")]
		public AudioClipAdditionalData audioClipAdditionalData;
	}

	[Serializable]
	public struct AudioClipAdditionalData
	{
		[InspectorReadonly] public float length;
		[InspectorReadonly] public string extension;
		[InspectorReadonly] public string importPath;
		[InspectorReadonly] public int channel;
		[InspectorReadonly] public int samplingFrequency;
		[InspectorReadonly] public int sampleCount;
		[InspectorReadonly] public int instanceId;

		public static AudioClipAdditionalData Reset()
		{
			return new AudioClipAdditionalData()
			{
				channel = 0,
				extension = string.Empty,
				importPath = string.Empty,
				length = 0,
				sampleCount = 0,
				samplingFrequency = 0,
				instanceId = 0,
			};
		}
	}
	
	[InspectorHideScriptField]
	internal sealed class AudioData : ScriptableObject
	{
		public List<AudioDataType> audioClipDatas = new();
#if UNITY_EDITOR

		[MenuItem("Horang/Module/Audio/Create Audio Database", false, 1)]
		private static void CreateFile()
		{
			if (File.Exists(@"Assets/Resources/Audio Database.asset"))
			{
				Log.Print("The audio database asset is already exist.", LogPriority.Warning);

				var currentAsset = AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/Resources/Audio Database.asset");

				PingAsset(currentAsset);
			}

			var instancedAsset = CreateInstance<AudioData>();
			AssetDatabase.CreateAsset(instancedAsset, @"Assets/Resources/Audio Database.asset");
			AssetDatabase.Refresh();

			PingAsset(instancedAsset);
		}

		private void OnValidate()
		{
			for (var index = 0; index < audioClipDatas.Count; index++)
			{
				try
				{
					var audioClipData = audioClipDatas[index];

					SetData(ref audioClipData);
					
					audioClipDatas[index] = audioClipData;
				}
				catch (Exception)
				{
					var audioClipData = audioClipDatas[index];

					ResetData(ref audioClipData);
					
					audioClipDatas[index] = audioClipData;
				}
			}
		}

		private static void SetData(ref AudioDataType aDT)
		{
			aDT.name = aDT.audioClip.name;

			aDT.audioClipAdditionalData.length = aDT.audioClip.length;
			aDT.audioClipAdditionalData.extension = Path.GetExtension(AssetDatabase.GetAssetPath(aDT.audioClip.GetInstanceID()));
			aDT.audioClipAdditionalData.importPath = AssetDatabase.GetAssetPath(aDT.audioClip.GetInstanceID());
			aDT.audioClipAdditionalData.channel = aDT.audioClip.channels;
			aDT.audioClipAdditionalData.samplingFrequency = aDT.audioClip.frequency;
			aDT.audioClipAdditionalData.sampleCount = aDT.audioClip.samples;
			aDT.audioClipAdditionalData.instanceId = aDT.audioClip.GetInstanceID();
		}

		private static void ResetData(ref AudioDataType aDT)
		{
			aDT.name = string.Empty;
			aDT.audioPlayType = AudioDataType.AudioPlayType.None;
			aDT.audioClipVolume = 1.0f;
			aDT.audioClipPan = 0.0f;

			aDT.audioClipAdditionalData = AudioClipAdditionalData.Reset();
		}

		private static void PingAsset(UnityEngine.Object obj)
		{
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = obj;
			EditorGUIUtility.PingObject(obj);
		}
#endif
	}
}