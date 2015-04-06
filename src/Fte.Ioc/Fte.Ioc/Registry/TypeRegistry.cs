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

		public void Register<TAbstraction, TConcrete>(LifeCycle lifeCycle) where TConcrete : TAbstraction
		{
			var abstractionType = typeof(TAbstraction);
			AssertTypeNotAlreadyRegistered(abstractionType);

			var concreteType = typeof(TConcrete);
			AssertTypeCanBeInstantiated(concreteType);

			var item = new TypeRegistryItem(abstractionType, concreteType, lifeCycle);
			_registeredTypes.Add(item);
        }

		public TypeRegistryItem GetRegistryItem(Type abstractionType)
		{
			if (abstractionType == null) throw new ArgumentNullException("abstractionType");

			var registryItem = _registeredTypes.FirstOrDefault(item => item.AbstractionType == abstractionType);

			if (registryItem == null)
			{
				throw new TypeNotRegisteredException(string.Format("Type {0} is not registered.", abstractionType.Name)); 
			}

			return registryItem;
		}

		private void AssertTypeNotAlreadyRegistered(Type abstractionType)
		{
			if (_registeredTypes.Any(x => x.AbstractionType == abstractionType))
			{
				throw new TypeAlreadyRegisteredException(string.Format("Type {0} is already registered.", abstractionType.Name));
			}
		}

		private void AssertTypeCanBeInstantiated(Type concreteType)
		{
			if (!concreteType.GetConstructors().Any())
			{
				throw new TypeCannotBeInstantiatedException(string.Format("Could not find any constructors for type {0}", concreteType.Name));
			}
		}
	}
}
