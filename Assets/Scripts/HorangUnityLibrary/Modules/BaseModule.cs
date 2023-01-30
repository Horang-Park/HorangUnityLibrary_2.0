using System;
using HorangUnityLibrary.Utilities;

namespace HorangUnityLibrary.Modules
{
	public abstract class BaseModule : IModule, IDisposable
	{
		public bool isRegistered;
		
		protected bool isThisModuleActivated;
		
		private bool isThisModuleInitialized;
		private readonly ModuleManager injectedModuleManager;

		/// <summary>
		/// Make module to active.
		/// </summary>
		/// <returns>If module activation working successfully, will return true</returns>
		public virtual bool ActiveModule()
		{
			if (ModuleValidateCheckOnActivate() is false)
			{
				return false;
			}
			
			injectedModuleManager.onInitializeOnce += InitializeOnce;
			injectedModuleManager.onInitializeLate += InitializeLate;
			injectedModuleManager.onUpdate += Update;
			injectedModuleManager.onFixedUpdate += FixedUpdate;
			injectedModuleManager.onLateUpdate += LateUpdate;

			isThisModuleActivated = true;

			injectedModuleManager.activatedModuleCount++;
			injectedModuleManager.activatedModules.Add(ToString());

			return true;
		}

		/// <summary>
		/// Make module to inactive.
		/// </summary>
		/// <returns>if module inactivation working successfully, will return true</returns>
		public virtual bool InactiveModule()
		{
			if (ModuleValidateCheckOnInactivate() is false)
			{
				return false;
			}

			injectedModuleManager.onInitializeOnce -= InitializeOnce;
			injectedModuleManager.onInitializeLate -= InitializeLate;
			injectedModuleManager.onUpdate -= Update;
			injectedModuleManager.onFixedUpdate -= FixedUpdate;
			injectedModuleManager.onLateUpdate -= LateUpdate;

			isThisModuleActivated = false;

			injectedModuleManager.activatedModuleCount--;
			injectedModuleManager.activatedModules.Remove(ToString());

			return true;
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

		protected BaseModule(ModuleManager moduleManager)
		{
			injectedModuleManager = moduleManager;
			
			// ReSharper disable once VirtualMemberCallInConstructor
			Log.Print($"{ToString()} module has been registered.");
		}

		protected bool ModuleValidateCheckOnActivate()
		{
			if (isRegistered is false)
			{
				Log.Print($"{ToString()} module are not registered.", LogPriority.Error);
				
				return false;
			}
			
			if (isThisModuleActivated)
			{
				Log.Print($"{ToString()} module are already activated.", LogPriority.Warning);
				
				return false;
			}

			return true;
		}
		
		protected bool ModuleValidateCheckOnInactivate()
		{
			if (isRegistered is false)
			{
				Log.Print($"{ToString()} module are not registered.", LogPriority.Error);
				
				return false;
			}
			
			if (isThisModuleActivated is false)
			{
				Log.Print($"{ToString()} module are already inactivated.", LogPriority.Warning);
				
				return false;
			}

			return true;
		}

		public void Dispose()
		{
			Log.Print($"{ToString()} module are disposed.");
		}
	}
}