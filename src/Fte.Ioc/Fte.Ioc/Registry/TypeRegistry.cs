using Fte.Ioc.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Fte.Ioc.Registry
{
	internal class TypeRegistry : ITypeRegistry
	{
		private readonly IList<TypeRegistryItem> _registeredTypes;

		public TypeRegistry()
		{
			_registeredTypes = new List<TypeRegistryItem>();
		}

		public void Register<TAbstraction, TConcrete>(LifeCycle lifeCycle)
		{
			var item = new TypeRegistryItem(typeof(TAbstraction), typeof(TConcrete), lifeCycle);

			if (_registeredTypes.Any(x => x.AbstractionType == typeof(TAbstraction)))
			{
				throw new TypeAlreadyRegisteredException(string.Format("Type {0} is already registered.", typeof(TAbstraction).Name));
			}
			
			_registeredTypes.Add(item);
        }

		public TypeRegistryItem GetRegistryItem(Type abstractionType)
		{
			var registryItem = _registeredTypes.FirstOrDefault(item => item.AbstractionType == abstractionType);

			if (registryItem == null)
			{
				throw new TypeNotRegisteredException(string.Format("Type {0} is not registered.", abstractionType.Name)); 
			}

			return registryItem;
		}
	}
}
