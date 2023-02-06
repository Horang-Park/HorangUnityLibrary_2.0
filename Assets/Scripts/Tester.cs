using System;
using Horang.HorangUnityLibrary.Managers.Module;
using Horang.HorangUnityLibrary.Modules.AudioModule;
using Horang.HorangUnityLibrary.Modules.StopwatchModule;
using Horang.HorangUnityLibrary.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class Tester : MonoBehaviour
{
	private StopwatchModule stopwatchModule;
	private AudioModule audioModule;

	public Image progress;

	private const string SampleString = "Hello, World!";
	private string encryptString;

	private void Awake()
	{
		encryptString = Encryption.Encrypt(SampleString).ToLog();
	}

	private void Start()
	{
		ModuleManager.Instance.RegisterModule(new StopwatchModule(ModuleManager.Instance));
		stopwatchModule = ModuleManager.Instance.GetModule<StopwatchModule>();
		
		ModuleManager.Instance.RegisterModule(new AudioModule(ModuleManager.Instance));
		audioModule = ModuleManager.Instance.GetModule<AudioModule>();
		audioModule.isModuleCanBeUnregister = false;
		audioModule.ActiveModule();

		Encryption.Decrypt(encryptString).ToLog();
	}

	private void OnPlayAudio() // f1
	{
		audioModule.Play("kamp_eff_apply_item", f => { progress.fillAmount = f / audioModule.GetAudioLength("kamp_eff_apply_item"); });
	}

	private void OnPauseAudio()
	{
		audioModule.Pause("kamp_eff_apply_item");
	}

	private void OnResumeAudio()
	{
		audioModule.Resume("kamp_eff_apply_item");
	}

	private void OnStopAudio() // f4
	{
		audioModule.Stop("kamp_eff_apply_item");
	}

	private void OnDestroyAudioSources() // f5
	{
		audioModule.RemoveAllAudioSources();
	}
}