using UnityEditor;
using UnityEditor.SceneManagement;

namespace Editor.Tools
{
	public static class RunFromSpecificScene
	{
		[MenuItem("Horang/Tools/Debug Mode/Enter Playmode/From Start Scene", false, 1)]
		public static void SetupFromStartScene()
		{
			var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
			var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);

			EditorSceneManager.playModeStartScene = sceneAsset;
			EditorApplication.isPlaying = true;
		}

		[MenuItem("Horang/Tools/Debug Mode/Enter Playmode/From Current Scene", false, 1)]
		public static void StartFromThisScene()
		{
			EditorSceneManager.playModeStartScene = null;
			EditorApplication.isPlaying = true;
		}
	}
}