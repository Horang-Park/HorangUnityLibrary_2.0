using HorangUnityLibrary.Modules;
using HorangUnityLibrary.Utilities;

namespace HorangUnityLibrary
{
	public class ModuleExample : BaseModule
	{
		public ModuleExample(ModuleManager moduleManager) : base(moduleManager)
		{
			Log.Print("\"Requester\" module has been registered.");
		}

		public override bool ActiveModule()
		{
			if (base.ActiveModule() is false)
			{
				return false;
			}
			
			Log.Print("Module are activated");

			return true;
		}

		public override bool InactiveModule()
		{
			if (base.InactiveModule() is false)
			{
				return false;
			}
			
			Log.Print("Module are inactivated");

			return true;
		}

		public void DoSomething()
		{
			if (isThisModuleActivated is false)
			{
				return;
			}
			
			Log.Print("DO SOMETHING");
		}
	}
}