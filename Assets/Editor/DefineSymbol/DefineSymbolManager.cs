using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

namespace Editor.DefineSymbol
{
	public static class DefineSymbolManager
	{
		public static void EnableSymbol(string symbol, string informationText = null)
		{
			var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
			var allDefines = GetEverySymbols();

			allDefines.Add(symbol);

			PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, string.Join(DefineSymbols.DefineSeparator, allDefines.ToArray()));
			
			if (string.IsNullOrEmpty(informationText) is false)
			{
				EditorUtility.DisplayDialog("Information", informationText, "OK");
			}
		}

		public static void DisableSymbol(string symbol, string informationText = null)
		{
			var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
			var allDefines = GetEverySymbols();

			allDefines.Remove(symbol);

			PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, string.Join(DefineSymbols.DefineSeparator, allDefines.ToArray()));
			
			if (string.IsNullOrEmpty(informationText) is false)
			{
				EditorUtility.DisplayDialog("Information", informationText, "OK");
			}
		}

		public static bool IsAlreadyDefined(string symbol)
		{
			var allDefines = GetEverySymbols();

			return allDefines.Contains(symbol);
		}
		
		private static List<string> GetEverySymbols()
		{
			var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
			var currentDefines = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
			
			return currentDefines.Split(DefineSymbols.DefineSeparator).ToList();
		}
	}
}