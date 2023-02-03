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

		public void Play(string name, Action<float> onPlayTime = null)
		{
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

		public void Stop(string name)
		{
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

		public void Pause(string name)
		{
			var key = name.GetHashCode();

			if (ValidateAudioSource(key) is false)
			{
				Log.Print($"Cannot find audio source named [{name}].", LogPriority.Error);

				return;
			}

			var audioSource = audioSources[key];
			
			audioSource.Pause();
		}

		public void Resume(string name)
		{
			var key = name.GetHashCode();

			if (ValidateAudioSource(key) is false)
			{
				Log.Print($"Cannot find audio source named [{name}].", LogPriority.Error);

				return;
			}

			var audioSource = audioSources[key];
			
			audioSource.UnPause();
		}

		public float GetAudioLength(string name)
		{
			var key = name.GetHashCode();

			if (ValidateAudioClip(key) is false)
			{
				Log.Print($"Cannot find audio data named [{name}]. Check your Audio Database.asset file.", LogPriority.Error);
			}

			var audioData = audioDatas[key];

			return audioData.audioClipAdditionalData.length;
		}

		public void RemoveAllAudioSources()
		{
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