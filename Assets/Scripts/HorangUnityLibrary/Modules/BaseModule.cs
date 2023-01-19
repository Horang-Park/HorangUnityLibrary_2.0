using System;
using HorangUnityLibrary.Utilities;
using UniRx;

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

		public virtual void UseModule()
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
			if (isThisModuleInitialized)
			{
				return;
			}
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