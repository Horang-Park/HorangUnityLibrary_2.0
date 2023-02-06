using Horang.HorangUnityLibrary.Managers.Module;
using Horang.HorangUnityLibrary.Modules.AudioModule;
using Horang.HorangUnityLibrary.Modules.CameraModule;
using Horang.HorangUnityLibrary.Modules.ExternalApplicationLaunchModule;
using Horang.HorangUnityLibrary.Modules.StopwatchModule;
using Plugins.Android;
using UnityEngine;
using UnityEngine.UI;

public class Tester : MonoBehaviour
{
	private StopwatchModule stopwatchModule;
	private AudioModule audioModule;
	private CameraModule cameraModule;
	private ExternalApplicationLaunchModule externalApplicationLaunchModule;

	public Image progress;

	private void Start()
	{
		ModuleManager.Instance.RegisterModule(new StopwatchModule(ModuleManager.Instance));
		stopwatchModule = ModuleManager.Instance.GetModule<StopwatchModule>();

		ModuleManager.Instance.RegisterModule(new AudioModule(ModuleManager.Instance));
		audioModule = ModuleManager.Instance.GetModule<AudioModule>();
		audioModule.ActiveModule();

		ModuleManager.Instance.RegisterModule(new CameraModule(ModuleManager.Instance));
		cameraModule = ModuleManager.Instance.GetModule<CameraModule>();
		cameraModule.ActiveModule();

		ModuleManager.Instance.RegisterModule(new ExternalApplicationLaunchModule(ModuleManager.Instance));
		externalApplicationLaunchModule = ModuleManager.Instance.GetModule<ExternalApplicationLaunchModule>();
		externalApplicationLaunchModule.ActiveModule();
	}

	private void OnPlayAudio() // f1
	{
		AndroidExtensionForUnity.SendBroadcast("com.kumsung.logout", "com.kumsung.kamping.launcher");
	}

	private void OnPauseAudio() // f2
	{
	}

	private void OnResumeAudio() // f3
	{
	}

	private void OnStopAudio() // f4
	{
	}

	private void OnDestroyAudioSources() // f5
	{
	}
}