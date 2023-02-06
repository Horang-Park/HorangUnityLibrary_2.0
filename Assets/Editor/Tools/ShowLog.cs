using Editor.DefineSymbol;
using UnityEditor;

namespace Editor.Tools
{
	public abstract class ShowLog
	{
		[MenuItem("Horang/Tools/Debug Mode/Log/Enable")]
		private static void EnableLog()
		{
			DefineSymbolManager.EnableSymbol(DefineSymbols.LogSymbol, "Log enabled.\nPlease enter playmode after Unity's code completion.");
		}

		[MenuItem("Horang/Tools/Debug Mode/Log/Enable", true)]
		private static bool ValidateEnableLog()
		{
			return DefineSymbolManager.IsAlreadyDefined(DefineSymbols.LogSymbol) == false;
		}

		[MenuItem("Horang/Tools/Debug Mode/Log/Disable")]
		private static void DisableLog()
		{
			DefineSymbolManager.DisableSymbol(DefineSymbols.LogSymbol, "Log disabled.\nPlease enter playmode after Unity's code completion.");
		}

		[MenuItem("Horang/Tools/Debug Mode/Log/Disable", true)]
		private static bool ValidateDisableLog()
		{
			return DefineSymbolManager.IsAlreadyDefined(DefineSymbols.LogSymbol);
		}
	}
}