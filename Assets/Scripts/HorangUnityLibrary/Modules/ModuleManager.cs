using System;
using System.Collections.Generic;
using System.Linq;
using HorangUnityLibrary.Utilities;
using JetBrains.Annotations;
using UnityEngine;

namespace HorangUnityLibrary.Modules
{
	public class ModuleManager : MonoBehaviour
	{
		[InspectorReadonly] public int registeredModuleCount;
		[InspectorReadonly] public List<string> registeredModules = new();
		[InspectorReadonly] public int activatedModuleCount;
		[InspectorReadonly] public List<string> activatedModules = new();

		private readonly Dictionary<Type, BaseModule> modules = new();

		public Action onInitializeOnce;
		public Action onInitializeLate;
		public Action onUpdate;
		public Action onFixedUpdate;
		public Action onLateUpdate;

		public void AddModule(BaseModule baseModule)
		{
			var key = baseModule.GetType();

			if (ValidateModuleExist(key))
			{
				Log.Print($"[{key}] module already exist.", LogPriority.Error);

				return;
			}

			modules.Add(key, baseModule);
			
			UpdateInspector();
		}

		public void RemoveModule(Type type)
		{
			if (ValidateModuleExist(type) is false)
			{
				Log.Print($"Cannot find [{type}] module.", LogPriority.Error);
				
				return;
			}
			
			modules.Remove(type);
			
			UpdateInspector();
		}

		[CanBeNull]
		public T GetModule<T>(Type type) where T : BaseModule
		{
			if (ValidateModuleExist(type))
			{
				return modules[type] as T;
			}
			
			Log.Print($"Cannot find [{type}] module.", LogPriority.Error);

			return null;
		}

		private void Awake()
		{
			gameObject.hideFlags = HideFlags.NotEditable;
			
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