using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HorangUnityLibrary.Modules;
using HorangUnityLibrary.Utilities.CustomAttribute;
using HorangUnityLibrary.Utilities.Foundation;
using JetBrains.Annotations;
using UnityEngine;

namespace HorangUnityLibrary.Utilities
{
	public class RmiManager : BaseManager<RmiManager>
	{
		private readonly Dictionary<int, (object, MethodBase)> rmiMethods = new();

		protected override void Awake()
		{
			base.Awake();
			
			GetRmiMethods();
		}

		[CanBeNull]
		public object Run(Type t, string methodName, params object[] parameters)
		{
			var key = t.GetHashCode() & methodName.GetHashCode();
			
			if (MethodValidation(key) is false)
			{
				Log.Print($"The method [{methodName}] within the type [{t}] is not exist.", LogPriority.Error);

				return null;
			}
			
			var runInstanceAndMethod = rmiMethods[key];
			
			Log.Print($"RMI Called - To: {t}, To call method: {runInstanceAndMethod.Item2}", LogPriority.Verbose);
			
			var moduleInstance = ModuleManager.Instance.GetModule<BaseModule>(t, true);

			if (moduleInstance is not null)
			{
				if (rmiMethods[key].Item1 is not null)
				{
					return runInstanceAndMethod.Item2.Invoke(runInstanceAndMethod.Item1, parameters);
				}
				
				// instance caching
				var moduleMethodNewItem = rmiMethods[key];
				moduleMethodNewItem.Item1 = moduleInstance;
				rmiMethods[key] = moduleMethodNewItem;

				return moduleMethodNewItem.Item2.Invoke(moduleMethodNewItem.Item1, parameters);
			}
			
			// If is not module method, trying to find Unity component instance.
			// Should be same game object name with the type.
			var unityGameObject = GameObject.Find(t.ToString());
			
			if (unityGameObject is not null)
			{
				var unityInstance = unityGameObject.GetComponent(t);
				
				if (rmiMethods[key].Item1 is not null)
				{
					return runInstanceAndMethod.Item2.Invoke(runInstanceAndMethod.Item1, parameters);
				}

				// instance caching
				var unityMethodNewItem = rmiMethods[key];
				unityMethodNewItem.Item1 = unityInstance;
				rmiMethods[key] = unityMethodNewItem;

				return unityMethodNewItem.Item2.Invoke(unityMethodNewItem.Item1, parameters);
			}
			
			Log.Print($"Cannot create instance of type [{t}].\nCheck the module are registered or game object name is same with the type [{t}].", LogPriority.Exception);

			throw new RmiException();
		}

		private bool MethodValidation(int k)
		{
			return rmiMethods.ContainsKey(k);
		}

		private void GetRmiMethods()
		{
			var executingAssembly = Assembly.GetExecutingAssembly();
			var types = executingAssembly.GetTypes();

			foreach (var type in types)
			{
				var methodInfos = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
					.Where(mi => mi.GetCustomAttribute<RMI>() is not null)
					.ToList();

				foreach (var mi in methodInfos)
				{
					var key = type.GetHashCode() & mi.Name.GetHashCode();
					
					rmiMethods.Add(key, (null, mi));
				}
			}
		}
	}

	public class RmiException : Exception { }
}