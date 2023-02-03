using Editor.DefineSymbol;
using HorangUnityLibrary.Utilities;
using UnityEditor;

namespace Editor.Tools
{
	public abstract class ShowLog
	{
		[MenuItem("Horang/Tools/Debug Mode/Log/Enable")]
		private static void EnableLog()
		{
			DefineSymbolManager.EnableSymbol(DefineSymbols.LogSymbol, "로그가 활성화 되었어요.\n스크립트 처리 후 플레이 해주세요.");
		}

		[MenuItem("Horang/Tools/Debug Mode/Log/Enable", true)]
		private static bool ValidateEnableLog()
		{
			return DefineSymbolManager.IsAlreadyDefined(DefineSymbols.LogSymbol) == false;
		}

		[MenuItem("Horang/Tools/Debug Mode/Log/Disable")]
		private static void DisableLog()
		{
			DefineSymbolManager.DisableSymbol(DefineSymbols.LogSymbol, "로그가 비활성화 되었어요.\n스크립트 처리 후 플레이 해주세요.");
		}

		[MenuItem("Horang/Tools/Debug Mode/Log/Disable", true)]
		private static bool ValidateDisableLog()
		{
			return DefineSymbolManager.IsAlreadyDefined(DefineSymbols.LogSymbol);
		}
	}
}