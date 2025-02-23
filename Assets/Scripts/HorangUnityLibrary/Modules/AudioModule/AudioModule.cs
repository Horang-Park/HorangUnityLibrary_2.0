using System;
using System.Collections.Generic;
using Horang.HorangUnityLibrary.Utilities;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Horang.HorangUnityLibrary.Modules.AudioModule
{
    public static class AudioModule
    {
        private static Transform _parent;

        private static readonly Dictionary<int, AudioDataType> AudioData = new();
        private static readonly Dictionary<int, AudioSource> AudioSources = new();
        private static readonly Dictionary<AudioDataType.AudioPlayType, List<AudioSource>> AudioSourcesByCategory = new();
        private static readonly Dictionary<int, IDisposable> AudioSourceTimeSubscribers = new();
        private static readonly Dictionary<AudioDataType.AudioPlayType, float> VolumeStatus = new();
        private static readonly Dictionary<AudioDataType.AudioPlayType, bool> MuteStatus = new();

        private const string ParentGameObjectName = "Audio Sources";

        public static void OnInitialize()
        {
            _parent = new GameObject(ParentGameObjectName).transform;
            _parent.gameObject.hideFlags = HideFlags.NotEditable;

            Object.DontDestroyOnLoad(_parent);

            LoadData();
        }

        public static void Dispose()
        {
            AudioSources.Clear();
            AudioSourceTimeSubscribers.Clear();

            RemoveAllAudioSources();
        }

        /// <summary>
        /// Play audio that in audio database
        /// </summary>
        /// <param name="name">To play audio clip name</param>
        /// <param name="onPlayTime">Callback audio source play time</param>
        public static void Play(string name, Action<float> onPlayTime = null)
        {
            var key = name.GetHashCode();

            if (ValidateAudioClip(key) is false)
            {
                Log.Print($"Cannot find audio data named [{name}]. Check your Audio Database.asset file.", LogPriority.Error);

                return;
            }

            var audioData = AudioData[key];
            var audioSource = GetOrCreateInstance(name);

            if (onPlayTime is not null)
            {
                if (ValidateAudioPlayTimeSubscriber(key))
                {
                    AudioSourceTimeSubscribers[key]?.Dispose();
                    AudioSourceTimeSubscribers[key] = Observable.EveryUpdate()
                        .Select(_ => audioSource.time)
                        .DistinctUntilChanged()
                        .Subscribe(onPlayTime)
                        .AddTo(audioSource.gameObject);
                }
                else
                {
                    AudioSourceTimeSubscribers.Add(key, Observable.EveryUpdate()
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
        public static void Stop(string name)
        {
            var key = name.GetHashCode();

            if (ValidateAudioSource(key) is false)
            {
                Log.Print($"Cannot find audio source named [{name}].", LogPriority.Error);

                return;
            }

            var audioSource = AudioSources[key];

            audioSource.Stop();

            if (ValidateAudioPlayTimeSubscriber(key) is false)
            {
                return;
            }

            AudioSourceTimeSubscribers[key]?.Dispose();
            AudioSourceTimeSubscribers.Remove(key);
        }

        /// <summary>
        /// Pause audio
        /// </summary>
        /// <param name="name">To pause audio clip name</param>
        public static void Pause(string name)
        {
            var key = name.GetHashCode();

            if (ValidateAudioSource(key) is false)
            {
                Log.Print($"Cannot find audio source named [{name}].", LogPriority.Error);

                return;
            }

            var audioSource = AudioSources[key];

            audioSource.Pause();
        }

        /// <summary>
        /// Resume(Unpause) audio
        /// </summary>
        /// <param name="name">To resume(unpause) audio clip name</param>
        public static void Resume(string name)
        {
            var key = name.GetHashCode();

            if (ValidateAudioSource(key) is false)
            {
                Log.Print($"Cannot find audio source named [{name}].", LogPriority.Error);

                return;
            }

            var audioSource = AudioSources[key];

            audioSource.UnPause();
        }

        /// <summary>
        /// Change audio source volume
        /// </summary>
        /// <param name="name">To change audio source name</param>
        /// <param name="volume">To set volume</param>
        public static void Volume(string name, float volume)
        {
            var key = name.GetHashCode();

            if (ValidateAudioSource(key) is false)
            {
                Log.Print($"Cannot find audio source named [{name}].", LogPriority.Error);

                return;
            }

            if (volume is > 1.0f or < 0.0f)
            {
                Log.Print($"Cannot set audio volume in [{volume}]. The range is 0.0f ~ 1.0f", LogPriority.Error);

                return;
            }

            var audioSource = AudioSources[key];

            audioSource.volume = volume;
        }

        /// <summary>
        /// Set volume by category
        /// </summary>
        /// <param name="audioPlayType">To set volume audio sources type</param>
        /// <param name="volume">To set volume (0.0~1.0)</param>
        public static void VolumeByCategory(AudioDataType.AudioPlayType audioPlayType, float volume = 1.0f)
        {
            if (AudioSourcesByCategory.TryGetValue(audioPlayType, out var items) is false)
            {
                Log.Print($"The audio play type [{audioPlayType}] is not in Audio Database.", LogPriority.Error);

                return;
            }

            foreach (var item in items)
            {
                item.volume = volume;
            }

            if (VolumeStatus.TryAdd(audioPlayType, volume) is false)
            {
                VolumeStatus[audioPlayType] = volume;
            }
        }

        /// <summary>
        /// Get audio clip's length
        /// </summary>
        /// <param name="name">To get audio clip name</param>
        /// <returns>Audio clip length</returns>
        public static float GetAudioLength(string name)
        {
            var key = name.GetHashCode();

            if (ValidateAudioClip(key) is false)
            {
                Log.Print($"Cannot find audio data named [{name}]. Check your [Audio Database.asset] file.", LogPriority.Error);
            }

            var audioData = AudioData[key];

            return audioData.audioClipAdditionalData.length;
        }

        /// <summary>
        /// Get named audio source is playing currently
        /// </summary>
        /// <param name="name">To get audio clip name</param>
        /// <returns>Named audio source is playing</returns>
        public static bool IsAudioPlaying(string name)
        {
            var key = name.GetHashCode();

            if (ValidateAudioClip(key) is false)
            {
                Log.Print($"Cannot find audio data named [{name}]. Check your Audio Database.asset file.", LogPriority.Error);

                return default;
            }

            var audioSource = GetOrCreateInstance(name);

            return audioSource.isPlaying;
        }

        /// <summary>
        /// Mute all audio sources by category
        /// </summary>
        /// <param name="audioPlayType">To mute audio sources type</param>
        public static void MuteByCategory(AudioDataType.AudioPlayType audioPlayType)
        {
            if (AudioSourcesByCategory.TryGetValue(audioPlayType, out var items) is false)
            {
                Log.Print($"The audio play type [{audioPlayType}] is not in Audio Database.", LogPriority.Error);

                return;
            }

            foreach (var item in items)
            {
                item.mute = true;
            }

            MuteStatus.TryAdd(audioPlayType, true);
        }

        /// <summary>
        /// Unmute all audio source by category
        /// </summary>
        /// <param name="audioPlayType">To unmute audio sources type</param>
        public static void UnmuteByCategory(AudioDataType.AudioPlayType audioPlayType)
        {
            if (AudioSourcesByCategory.ContainsKey(audioPlayType) is false)
            {
                Log.Print($"The audio play type [{audioPlayType}] is not in Audio Database.", LogPriority.Error);

                return;
            }

            var items = AudioSourcesByCategory[audioPlayType];

            foreach (var item in items)
            {
                item.mute = false;
            }

            MuteStatus.Remove(audioPlayType);
        }

        /// <summary>
        /// Toggle mute by audio source name
        /// </summary>
        /// <param name="name">To set mute or unmute audio source name</param>
        public static void ToggleMuteByName(string name)
        {
            var key = name.GetHashCode();

            if (ValidateAudioClip(key) is false)
            {
                Log.Print($"Cannot find audio data named [{name}]. Check your Audio Database.asset file.", LogPriority.Error);

                return;
            }

            var audioSource = GetOrCreateInstance(name);

            audioSource.mute = !audioSource.mute;
        }

        private static void LoadData()
        {
            var audioDataScriptableObject = Resources.Load<AudioData>("Audio Database");

            if (audioDataScriptableObject is null)
            {
                Log.Print("Cannot find audio database scriptable object. Create audio database asset in [Resources/Audios] and rerun application.", LogPriority.Error);

                return;
            }

            foreach (var audioData in audioDataScriptableObject.audioClipDatas)
            {
                AudioData.Add(audioData.name.GetHashCode(), audioData);

                if (AudioSourcesByCategory.ContainsKey(audioData.audioPlayType) is false)
                {
                    AudioSourcesByCategory.Add(audioData.audioPlayType, new List<AudioSource>());
                }
            }
        }

        private static AudioSource GetOrCreateInstance(string n)
        {
            var key = n.GetHashCode();

            if (ValidateAudioSource(key))
            {
                return AudioSources[key];
            }

            Log.Print($"Create new game object with audio source by name [{n}]", LogPriority.Verbose);

            var go = new GameObject(n);
            go.transform.SetParent(_parent);
            go.hideFlags = HideFlags.NotEditable;

            var co = go.AddComponent(typeof(AudioSource)) as AudioSource;
            var ad = AudioData[key];

            AudioSources.Add(key, co);
            AudioSourcesByCategory[ad.audioPlayType].Add(co);

            if (MuteStatus.TryGetValue(ad.audioPlayType, out var isMute))
            {
                co!.mute = isMute;
            }

            if (VolumeStatus.TryGetValue(ad.audioPlayType, out var volume))
            {
                co!.volume = volume;
            }

            return AudioSources[key];
        }

        private static bool ValidateAudioClip(int n)
        {
            return AudioData.ContainsKey(n.GetHashCode());
        }

        private static bool ValidateAudioSource(int n)
        {
            return AudioSources.ContainsKey(n);
        }

        private static bool ValidateAudioPlayTimeSubscriber(int n)
        {
            return AudioSourceTimeSubscribers.ContainsKey(n);
        }
        
        private static void RemoveAllAudioSources()
        {
            foreach (var audioSource in AudioSources)
            {
                Object.Destroy(audioSource.Value.gameObject);
            }

            AudioSources.Clear();
            AudioSourceTimeSubscribers.Clear();
        }
    }
}