using System;
using System.Collections.Generic;
using System.Reflection;

namespace MBEasyMod.Services
{
	public class ServiceManager
	{
		private static Dictionary<Type, dynamic> registeredServices = new Dictionary<Type, dynamic>();

		public static void RegisterService<T, G>(G service = default)
		{
			if(registeredServices.ContainsKey(typeof(T)))
				throw new InvalidOperationException($"A service of type {typeof(T)} has already been registered!");

			if(!service.Equals(default))
			{
				registeredServices.Add(typeof(T), service);
				return;
			}

			ConstructorInfo ctor = typeof(T).GetConstructor(null);
			if(ctor == null)
				throw new MissingMethodException($"{typeof(T)} does not contain a parameterless constuctor! If you wish to add a service without a parameterless constructor please provide an instance.");
			registeredServices.Add(typeof(T), ctor.Invoke(null));
		}

		public static bool TryGetService<T>(out T service)
		{
			service = default;
			if(!registeredServices.ContainsKey(typeof(T)))
				return false;
			
			service = registeredServices[typeof(T)];
			return true;
		}

		public static void DeregisterService<T, G>(G service = default)
		{
			if(!registeredServices.ContainsKey(typeof(T)))
				return;
			
			registeredServices.Remove(typeof(T));
		}
	}
}
