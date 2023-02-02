using Cysharp.Threading.Tasks;
using HorangUnityLibrary.Managers.Module;
using HorangUnityLibrary.Modules.StopwatchModule;
using HorangUnityLibrary.Utilities;
using UnityEngine;

public class Tester : MonoBehaviour
{
	private StopwatchModule stopwatchModule;

	private void Start()
	{
		ModuleManager.Instance.RegisterModule(new StopwatchModule(ModuleManager.Instance));
		stopwatchModule = ModuleManager.Instance.GetModule<StopwatchModule>(typeof(StopwatchModule));
		stopwatchModule.ActiveModule();

		TestGetImageFromRemote().Forget();
	}

	private static async UniTaskVoid TestGetImageFromRemote()
	{
		await ImageLoader.LoadManyFromRemote(new[]
		{
			"https://webb.nasa.gov/content/webbLaunch/assets/images/images2023/jan-31-23-potm2301a-4k.jpg",
			"https://upload.wikimedia.org/wikipedia/commons/7/7f/%22A_Lie%22_Steel_head_answers_to_Senator_Guffey%27s_questions._Washington%2C_D.C.%2C_June_24._Senator_Joseph_Goffey_of_Pennsylvania_as_he_questioned_Tom_Girdler%2C_Head_of_Republic_Steel_Corporation%2C_LCCN2016871895.jpg",
		});
	}
}