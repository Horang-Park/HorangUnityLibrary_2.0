using System;
using System.Collections.Generic;
using HorangUnityLibrary.Foundation.Module;
using HorangUnityLibrary.Managers.Module;
using HorangUnityLibrary.Utilities;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HorangUnityLibrary.Modules.AudioModule
{
	public class AudioModule : BaseModule
	{
		private Transform parent;
		
		private readonly Dictionary<int, AudioDataType> audioDatas = new();
		private readonly Dictionary<int, AudioSource> audioSources = new();
		private readonly Dictionary<int, IDisposable> audioSourceTimeSubscribers = new();

		private const string ParentGameObjectName = "Audio Sources";

		public AudioModule(ModuleManager moduleManager) : base(moduleManager)
		{
		}

		public override bool ActiveModule()
		{
			if (base.ActiveModule() is false)
			{
				return false;
			}
			
			Log.Print("Module are activated", LogPriority.Verbose);

			return true;
		}

		public override bool InactiveModule()
		{
			if (base.InactiveModule() is false)
			{
				return false;
			}
			
			Log.Print("Module are inactivated", LogPriority.Verbose);

			return true;
		}

		public override void InitializeOnce()
		{
			base.InitializeOnce();

			parent = new GameObject(ParentGameObjectName).transform;
			parent.gameObject.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;

			LoadData();
		}

		public override void InitializeOnInactivateEverytime()
		{
			base.InitializeOnInactivateEverytime();

			audioSources.Clear();
			audioSourceTimeSubscribers.Clear();
			
			RemoveAllAudioSources();
		}

		/// <summary>
		/// Play audio that in audio database
		/// </summary>
		/// <param name="name">To play audio clip name</param>
		/// <param name="onPlayTime">Callback audio source play time</param>
		public void Play(string name, Action<float> onPlayTime = null)
		{
			if (isThisModuleActivated is false)
			{
				return;
			}
			
			var key = name.GetHashCode();

			if (ValidateAudioClip(key) is false)
			{
				Log.Print($"Cannot find audio data named [{name}]. Check your Audio Database.asset file.", LogPriority.Error);

				return;
			}

			var audioData = audioDatas[key];
			var audioSource = GetOrCreateInstance(name);
			
			if (ValidateAudioPlayTimeSubscriber(key))
			{
				audioSourceTimeSubscribers[key]?.Dispose();
				audioSourceTimeSubscribers[key] = Observable.EveryUpdate()
					.Select(_ => audioSource.time)
					.DistinctUntilChanged()
					.Subscribe(onPlayTime)
					.AddTo(audioSource.gameObject);
			}
			else
			{
				audioSourceTimeSubscribers.Add(key, Observable.EveryUpdate()
					.Select(_ => audioSource.time)
					.DistinctUntilChanged()
					.Subscribe(onPlayTime)
					.AddTo(audioSource.gameObject));
			}
			
			audioSource.Stop();

			audioSource.playOnAwake = false;
			audioSource.loop = audioData.audioPlayType is AudioDataType.AudioPlayType.LoopSfx or AudioDataType.AudioPlayType.BGM;
			audioSource.volume = audioData.audioClipVolume;
			audioSource.panStereo = audioData.audioClipPan;
			audioSource.clip = audioData.audioClip;

			audioSource.Play();
		}

		/// <summary>
		/// Stop audio 
		/// </summary>
		/// <param name="name">To stop audio clip name</param>
		public void Stop(string name)
		{
			if (isThisModuleActivated is false)
			{
				return;
			}
			
			var key = name.GetHashCode();

			if (ValidateAudioSource(key) is false
			    || ValidateAudioPlayTimeSubscriber(key) is false)
			{
				Log.Print($"Cannot find audio source named [{name}].", LogPriority.Error);

				return;
			}

			var audioSource = audioSources[key];
			
			audioSource.Stop();
			
			audioSourceTimeSubscribers[key]?.Dispose();
			audioSourceTimeSubscribers.Remove(key);
		}

		/// <summary>
		/// Pause audio
		/// </summary>
		/// <param name="name">To pause audio clip name</param>
		public void Pause(string name)
		{
			if (isThisModuleActivated is false)
			{
				return;
			}
			
			var key = name.GetHashCode();

			if (ValidateAudioSource(key) is false)
			{
				Log.Print($"Cannot find audio source named [{name}].", LogPriority.Error);

				return;
			}

			var audioSource = audioSources[key];
			
			audioSource.Pause();
		}

		/// <summary>
		/// Resume(Unpause) audio
		/// </summary>
		/// <param name="name">To resume(unpause) audio clip name</param>
		public void Resume(string name)
		{
			if (isThisModuleActivated is false)
			{
				return;
			}
			
			var key = name.GetHashCode();

			if (ValidateAudioSource(key) is false)
			{
				Log.Print($"Cannot find audio source named [{name}].", LogPriority.Error);

				return;
			}

			var audioSource = audioSources[key];
			
			audioSource.UnPause();
		}

		/// <summary>
		/// Get audio clip's length
		/// </summary>
		/// <param name="name">To get audio clip name</param>
		/// <returns>Audio clip length</returns>
		public float GetAudioLength(string name)
		{
			if (isThisModuleActivated is false)
			{
				return float.PositiveInfinity;
			}
			
			var key = name.GetHashCode();

			if (ValidateAudioClip(key) is false)
			{
				Log.Print($"Cannot find audio data named [{name}]. Check your Audio Database.asset file.", LogPriority.Error);
			}

			var audioData = audioDatas[key];

			return audioData.audioClipAdditionalData.length;
		}

		/// <summary>
		/// Remove created audio source game objects (except audio datas)
		/// </summary>
		public void RemoveAllAudioSources()
		{
			if (isThisModuleActivated is false)
			{
				return;
			}
			
			foreach (var audioSource in audioSources)
			{
				Object.Destroy(audioSource.Value.gameObject);
			}
			
			audioSources.Clear();
			audioSourceTimeSubscribers.Clear();
		}

		private void LoadData()
		{
			var audioDataScriptableObject = Resources.Load<AudioData>("Audios/Audio Database");

			if (audioDataScriptableObject is null)
			{
				Log.Print("Cannot find audio database scriptable object. Create audio database asset and rerun application.");
				
				return;
			}

			foreach (var audioData in audioDataScriptableObject.audioClipDatas)
			{
				audioDatas.Add(audioData.name.GetHashCode(), audioData);
			}
		}

		private AudioSource GetOrCreateInstance(string n)
		{
			var key = n.GetHashCode();

			if (ValidateAudioSource(key))
			{
				return audioSources[key];
			}
			
			Log.Print($"Create new game object with audio source by name [{n}]", LogPriority.Verbose);

			var go = new GameObject(n);
			go.transform.SetParent(parent);
			go.hideFlags = HideFlags.NotEditable;
			
			var co = go.AddComponent(typeof(AudioSource)) as AudioSource;
				
			audioSources.Add(key, co);

			return audioSources[key];
		}

		private bool ValidateAudioClip(int n)
		{
			return audioDatas.ContainsKey(n.GetHashCode());
		}

		private bool ValidateAudioSource(int n)
		{
			return audioSources.ContainsKey(n);
		}

		private bool ValidateAudioPlayTimeSubscriber(int n)
		{
			return audioSourceTimeSubscribers.ContainsKey(n);
		}
	}
}