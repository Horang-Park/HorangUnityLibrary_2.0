using System;
using System.Collections.Generic;
using Horang.HorangUnityLibrary.Foundation.Module;
using Horang.HorangUnityLibrary.Managers.Module;
using Horang.HorangUnityLibrary.Utilities;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Horang.HorangUnityLibrary.Modules.AudioModule
{
	public sealed class AudioModule : BaseModule
	{
		private Transform parent;
		
		private readonly Dictionary<int, AudioDataType> audioDatas = new();
		private readonly Dictionary<int, AudioSource> audioSources = new();
		private readonly Dictionary<AudioDataType.AudioPlayType, List<AudioSource>> audioSourcesByCategory = new();
		private readonly Dictionary<int, IDisposable> audioSourceTimeSubscribers = new();
		private readonly Dictionary<AudioDataType.AudioPlayType, bool> muteStatus = new();

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
			parent.gameObject.hideFlags = HideFlags.NotEditable;

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
			
			if (onPlayTime is not null)
			{
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
				Log.Print($"Cannot find audio data named [{name}]. Check your [Audio Database.asset] file.", LogPriority.Error);
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

		public void MuteByCategory(AudioDataType.AudioPlayType audioPlayType)
		{
			if (isThisModuleActivated is false)
			{
				return;
			}

			if (audioSourcesByCategory.ContainsKey(audioPlayType) is false)
			{
				Log.Print($"The audio play type [{audioPlayType}] is not in Audio Database.", LogPriority.Error);

				return;
			}
			
			var items = audioSourcesByCategory[audioPlayType];

			foreach (var item in items)
			{
				item.mute = true;
			}
			
			muteStatus.TryAdd(audioPlayType, true);
		}

		public void UnmuteByCategory(AudioDataType.AudioPlayType audioPlayType)
		{
			if (isThisModuleActivated is false)
			{
				return;
			}
			
			if (audioSourcesByCategory.ContainsKey(audioPlayType) is false)
			{
				Log.Print($"The audio play type [{audioPlayType}] is not in Audio Database.", LogPriority.Error);

				return;
			}
			
			var items = audioSourcesByCategory[audioPlayType];

			foreach (var item in items)
			{
				item.mute = false;
			}

			muteStatus.Remove(audioPlayType);
		}

		private void LoadData()
		{
			var audioDataScriptableObject = Resources.Load<AudioData>("Audio Database");

			if (audioDataScriptableObject is null)
			{
				Log.Print("Cannot find audio database scriptable object. Create audio database asset in [Resources/Audios] and rerun application.", LogPriority.Error);
				
				return;
			}

			foreach (var audioData in audioDataScriptableObject.audioClipDatas)
			{
				audioDatas.Add(audioData.name.GetHashCode(), audioData);

				if (audioSourcesByCategory.ContainsKey(audioData.audioPlayType) is false)
				{
					audioSourcesByCategory.Add(audioData.audioPlayType, new List<AudioSource>());
				}
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
			
			Object.DontDestroyOnLoad(go);
			
			var co = go.AddComponent(typeof(AudioSource)) as AudioSource;
			var ad = audioDatas[key];
				
			audioSources.Add(key, co);
			audioSourcesByCategory[ad.audioPlayType].Add(co);

			if (muteStatus.ContainsKey(ad.audioPlayType))
			{
				co!.mute = true;
			}

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