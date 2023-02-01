using System;
using System.Collections.Generic;
using System.Linq;
using HorangUnityLibrary.Foundation.Manager;
using HorangUnityLibrary.Foundation.Module;
using HorangUnityLibrary.Utilities;
using HorangUnityLibrary.Utilities.CustomAttribute;
using JetBrains.Annotations;
using UnityEngine;

namespace HorangUnityLibrary.Managers.Module
{
	public class ModuleManager : SingletonBaseManager<ModuleManager>
	{
		[Header("Module Manager Status")]
		[InspectorReadonly]
		public int registeredModuleCount;
		[InspectorReadonly]
		public List<string> registeredModules = new();
		[InspectorReadonly]
		public int activatedModuleCount;
		[InspectorReadonly]
		public List<string> activatedModules = new();

		private readonly Dictionary<Type, BaseModule> modules = new();

		public Action onInitializeOnce;
		public Action onInitializeLate;
		public Action onUpdate;
		public Action onFixedUpdate;
		public Action onLateUpdate;

		/// <summary>
		/// Register module to module manager.
		/// </summary>
		/// <param name="baseModule">To register module</param>
		public void RegisterModule(BaseModule baseModule)
		{
			var key = baseModule.GetType();

			if (ValidateModuleExist(key))
			{
				Log.Print($"[{key}] module already exist.", LogPriority.Error);

				return;
			}

			modules.Add(key, baseModule);

			baseModule.isRegistered = true;
			
			UpdateInspector();
		}

		/// <summary>
		/// Unregister module from module manager.
		/// </summary>
		/// <param name="type">To unregistering module type</param>
		public void UnregisterModule(Type type)
		{
			if (ValidateModuleExist(type) is false)
			{
				Log.Print($"Cannot find [{type}] module.", LogPriority.Error);
				
				return;
			}

			var targetModule = modules[type];

			if (targetModule.isModuleCanBeUnregister is false)
			{
				Log.Print($"[{ToString()}] module cannot unregister.", LogPriority.Warning);
				
				return;
			}
			
			targetModule.InactiveModule();
			targetModule.isRegistered = false;
			targetModule.Dispose();
			
			modules.Remove(type);
			
			UpdateInspector();
		}

		/// <summary>
		/// To getting module that already registered.
		/// </summary>
		/// <param name="type">To get a module type</param>
		/// <typeparam name="T">Type that inheritance BaseModule</typeparam>
		/// <returns>Specific module or null</returns>
		[CanBeNull]
		public T GetModule<T>(Type type, bool useFromRmi = false) where T : BaseModule
		{
			if (ValidateModuleExist(type))
			{
				return modules[type] as T;
			}

			if (useFromRmi)
			{
				return null;
			}
			
			Log.Print($"Cannot find [{type}] module.", LogPriority.Error);

			return null;
		}

		protected override void Awake()
		{
			base.Awake();
			
			onInitializeOnce?.Invoke();
		}

		private void Start()
		{
			onInitializeLate?.Invoke();
		}

		private void Update()
		{
			onUpdate?.Invoke();
		}

		private void FixedUpdate()
		{
			onFixedUpdate?.Invoke();
		}

		private void LateUpdate()
		{
			onLateUpdate?.Invoke();
		}

		private bool ValidateModuleExist(Type t)
		{
			return modules.ContainsKey(t);
		}

		private void UpdateInspector()
		{
			registeredModuleCount = modules.Count;
			registeredModules = modules.Keys.Select(key => key.ToString()).ToList();
		}
	}
}