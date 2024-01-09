using UnityEditor;
using UnityEditor.SceneManagement;
// ReSharper disable CheckNamespace

namespace HorangEditor.Tools
{
	public static class RunFromSpecificScene
	{
		[MenuItem("Horang/Tools/Start Play Mode From/Start Scene", false, 0)]
		public static void SetupFromStartScene()
		{
			var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
			var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);

			EditorSceneManager.playModeStartScene = sceneAsset;
			EditorApplication.isPlaying = true;
		}

		[MenuItem("Horang/Tools/Start Play Mode From/Current Scene", false, 1)]
		public static void StartFromThisScene()
		{
			EditorSceneManager.playModeStartScene = null;
			EditorApplication.isPlaying = true;
		}
	}
}