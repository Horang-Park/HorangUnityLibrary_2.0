namespace HorangUnityLibrary.Modules
{
	public abstract class BaseModule : IModule
	{
		protected bool isThisModuleActivated;
		
		private bool isThisModuleInitialized;
		private readonly ModuleManager injectedModuleManager;

		protected BaseModule(ModuleManager moduleManager)
		{
			injectedModuleManager = moduleManager;
		}

		public virtual void ActiveModule()
		{
			if (isThisModuleActivated)
			{
				return;
			}
			
			injectedModuleManager.onInitializeOnce += InitializeOnce;
			injectedModuleManager.onInitializeLate += InitializeLate;
			injectedModuleManager.onUpdate += Update;
			injectedModuleManager.onFixedUpdate += FixedUpdate;
			injectedModuleManager.onLateUpdate += LateUpdate;

			isThisModuleActivated = true;

			injectedModuleManager.activatedModuleCount++;
			injectedModuleManager.activatedModules.Add(ToString());
		}

		public virtual void InactiveModule()
		{
			if (isThisModuleActivated is false)
			{
				return;
			}
			
			injectedModuleManager.onInitializeOnce -= InitializeOnce;
			injectedModuleManager.onInitializeLate -= InitializeLate;
			injectedModuleManager.onUpdate -= Update;
			injectedModuleManager.onFixedUpdate -= FixedUpdate;
			injectedModuleManager.onLateUpdate -= LateUpdate;

			isThisModuleActivated = false;

			injectedModuleManager.activatedModuleCount--;
			injectedModuleManager.activatedModules.Remove(ToString());
		}

		public virtual void InitializeOnce()
		{
			if (isThisModuleInitialized)
			{
				return;
			}
			
			isThisModuleInitialized = true;
		}

		public virtual void InitializeLate()
		{
		}

		public virtual void Update()
		{
		}

		public virtual void FixedUpdate()
		{
		}

		public virtual void LateUpdate()
		{
		}
	}
}