using HorangEditor.DefineSymbol;
using UnityEditor;
// ReSharper disable CheckNamespace

namespace HorangEditor.Tools
{
	public abstract class ShowLog
	{
		[MenuItem("Horang/Tools/Log/Enable", false, 0)]
		private static void EnableLog()
		{
			DefineSymbolManager.EnableSymbol(DefineSymbols.LogSymbol, "Log enabled.\nPlease enter playmode after Unity's code completion.");
		}

		[MenuItem("Horang/Tools/Log/Enable", true, 0)]
		private static bool ValidateEnableLog()
		{
			return DefineSymbolManager.IsAlreadyDefined(DefineSymbols.LogSymbol) == false;
		}

		[MenuItem("Horang/Tools/Log/Disable", false, 1)]
		private static void DisableLog()
		{
			DefineSymbolManager.DisableSymbol(DefineSymbols.LogSymbol, "Log disabled.\nPlease enter playmode after Unity's code completion.");
		}

		[MenuItem("Horang/Tools/Log/Disable", true, 1)]
		private static bool ValidateDisableLog()
		{
			return DefineSymbolManager.IsAlreadyDefined(DefineSymbols.LogSymbol);
		}
	}
}