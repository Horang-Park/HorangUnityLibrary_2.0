using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HorangUnityLibrary.Modules;
using HorangUnityLibrary.Utilities.CustomAttribute;
using HorangUnityLibrary.Utilities.Foundation;
using UnityEngine;

namespace HorangUnityLibrary.Utilities
{
	public class RMIManager : BaseManager<RMIManager>
	{
		private readonly Dictionary<int, (object, MethodBase)> rmiMethods = new();

		protected override void Awake()
		{
			base.Awake();
			
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

		public object Run(Type t, string methodName, params object[] parameters)
		{
			var key = t.GetHashCode() & methodName.GetHashCode();
			var runInstanceAndMethod = rmiMethods[key];
			var moduleInstance = ModuleManager.Instance.GetModule<BaseModule>(t, true);
			
			if (moduleInstance is not null)
			{
				if (rmiMethods[key].Item1 is not null)
				{
					return runInstanceAndMethod.Item2.Invoke(runInstanceAndMethod.Item1, parameters);
				}
				
				var moduleMethodNewItem = rmiMethods[key];
				moduleMethodNewItem.Item1 = moduleInstance;
				rmiMethods[key] = moduleMethodNewItem;

				return moduleMethodNewItem.Item2.Invoke(moduleMethodNewItem.Item1, parameters);
			}
			
			// If is not module method, trying to find Unity component instance.
			// Should be same name game object with the type.
			var unityInstance = GameObject.Find(t.ToString()).GetComponent(t);
			
			if (rmiMethods[key].Item1 is not null)
			{
				return runInstanceAndMethod.Item2.Invoke(runInstanceAndMethod.Item1, parameters);
			}
				
			var unityMethodNewItem = rmiMethods[key];
			unityMethodNewItem.Item1 = unityInstance;
			rmiMethods[key] = unityMethodNewItem;

			return unityMethodNewItem.Item2.Invoke(unityMethodNewItem.Item1, parameters);
		}
	}
}