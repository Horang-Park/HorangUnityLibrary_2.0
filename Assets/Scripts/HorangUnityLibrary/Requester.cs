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

		public override void UseModule()
		{
			if (isThisModuleActivated)
			{
				Log.Print("Already activated module.", LogPriority.Warning);
				
				return;
			}
			
			base.UseModule();
			
			Log.Print("\"Requester\" module has been set use mode.");
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