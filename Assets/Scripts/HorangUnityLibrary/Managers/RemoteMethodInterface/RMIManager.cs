using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HorangUnityLibrary.Modules;
using HorangUnityLibrary.Utilities;
using JetBrains.Annotations;
using UnityEngine;

namespace HorangUnityLibrary.Managers.RemoteMethodInterface
{
	public class RmiManager : BaseManager<RmiManager>
	{
		private readonly Dictionary<int, (object, MethodBase)> rmiMethods = new();

		protected override void Awake()
		{
			base.Awake();
			
			GetRmiMethods();
		}

		/// <summary>
		/// Call function by cached remote method interface.
		/// </summary>
		/// <param name="instanceType">The class or struct type of desire method is in</param>
		/// <param name="methodName">To call method name</param>
		/// <param name="parameters">To put parameters when call method</param>
		/// <returns>Will return specific value if validate method is success and method return type is not void. otherwise null or void</returns>
		/// <exception cref="RmiException">If cannot create class or struct instance. or cannot find module instance</exception>
		[CanBeNull]
		public object Run(Type instanceType, string methodName, params object[] parameters)
		{
			var key = UniqueHashKey(instanceType.GetHashCode(), methodName.GetHashCode());

			if (MethodValidation(key) is false)
			{
				Log.Print($"The method [{methodName}] within the type [{instanceType}] is not exist.", LogPriority.Error);

				return null;
			}
			
			var runInstanceAndMethod = rmiMethods[key];
			
			Log.Print($"RMI Called - To: {instanceType}, To call method: {runInstanceAndMethod.Item2}", LogPriority.Verbose);
			
			var moduleInstance = ModuleManager.Instance.GetModule<BaseModule>(instanceType, true);

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
			var unityGameObject = GameObject.Find(instanceType.ToString());
			
			if (unityGameObject is not null)
			{
				var unityInstance = unityGameObject.GetComponent(instanceType);
				
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
			
			Log.Print($"Cannot create instance of type [{instanceType}].\nCheck the module are registered or game object name is same with the type [{instanceType}].", LogPriority.Exception);

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
					.Where(mi => mi.GetCustomAttribute<RemoteMethodInterface.RMI>() is not null)
					.ToList();

				foreach (var mi in methodInfos)
				{
					var key = UniqueHashKey(type.GetHashCode(),mi.Name.GetHashCode());

					rmiMethods.Add(key, (null, mi));
				}
			}
		}

		private static int UniqueHashKey(int tH, int mnH)
		{
			return (tH << 0xf) ^ ~mnH;
		}
	}

	public class RmiException : Exception { }
}