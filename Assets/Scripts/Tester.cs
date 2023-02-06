using System;
using Horang.HorangUnityLibrary.Managers.Module;
using Horang.HorangUnityLibrary.Modules.AudioModule;
using Horang.HorangUnityLibrary.Modules.CameraModule;
using Horang.HorangUnityLibrary.Modules.StopwatchModule;
using Horang.HorangUnityLibrary.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class Tester : MonoBehaviour
{
	private StopwatchModule stopwatchModule;
	private AudioModule audioModule;
	private CameraModule cameraModule;

	public Image progress;

	private void Start()
	{
		ModuleManager.Instance.RegisterModule(new StopwatchModule(ModuleManager.Instance));
		stopwatchModule = ModuleManager.Instance.GetModule<StopwatchModule>();
		
		ModuleManager.Instance.RegisterModule(new AudioModule(ModuleManager.Instance));
		audioModule = ModuleManager.Instance.GetModule<AudioModule>();
		audioModule.isModuleCanBeUnregister = false;
		audioModule.ActiveModule();
		
		ModuleManager.Instance.RegisterModule(new CameraModule(ModuleManager.Instance));
		cameraModule = ModuleManager.Instance.GetModule<CameraModule>();
		cameraModule.isModuleCanBeUnregister = false;
		cameraModule.ActiveModule();
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