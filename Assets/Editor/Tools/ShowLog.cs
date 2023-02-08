using Editor.DefineSymbol;
using UnityEditor;

namespace Editor.Tools
{
	public abstract class ShowLog
	{
		[MenuItem("Horang/Tools/Debug Mode/Log/Enable", false, 0)]
		private static void EnableLog()
		{
			DefineSymbolManager.EnableSymbol(DefineSymbols.LogSymbol, "Log enabled.\nPlease enter playmode after Unity's code completion.");
		}

		[MenuItem("Horang/Tools/Debug Mode/Log/Enable", true, 0)]
		private static bool ValidateEnableLog()
		{
			return DefineSymbolManager.IsAlreadyDefined(DefineSymbols.LogSymbol) == false;
		}

		[MenuItem("Horang/Tools/Debug Mode/Log/Disable", false, 0)]
		private static void DisableLog()
		{
			DefineSymbolManager.DisableSymbol(DefineSymbols.LogSymbol, "Log disabled.\nPlease enter playmode after Unity's code completion.");
		}

		[MenuItem("Horang/Tools/Debug Mode/Log/Disable", true, 0)]
		private static bool ValidateDisableLog()
		{
			return DefineSymbolManager.IsAlreadyDefined(DefineSymbols.LogSymbol);
		}
	}
}