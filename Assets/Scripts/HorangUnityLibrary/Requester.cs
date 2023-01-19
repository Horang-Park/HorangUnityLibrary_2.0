using HorangUnityLibrary.Modules;
using HorangUnityLibrary.Utilities;

namespace HorangUnityLibrary
{
	public class Requester : BaseModule
	{
		public Requester(ModuleManager moduleManager) : base(moduleManager)
		{
			Log.Print("\"Requester\" module has been registered.");
		}

		public override void ActiveModule()
		{
			if (isThisModuleActivated)
			{
				Log.Print("Already activated module.", LogPriority.Warning);
				
				return;
			}
			
			base.ActiveModule();
			
			Log.Print("Module are activated");
		}

		public override void InactiveModule()
		{
			if (isThisModuleActivated is false)
			{
				Log.Print("Already inactivated module.", LogPriority.Warning);
				
				return;
			}
			
			base.InactiveModule();
			
			Log.Print("Module are inactivated");
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