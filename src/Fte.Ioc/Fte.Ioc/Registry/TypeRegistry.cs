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
			if (abstractionType == null) throw new ArgumentNullException(nameof(abstractionType));

			var registryItem = _registeredTypes.FirstOrDefault(item => item.AbstractionType == abstractionType);

			if (registryItem == null)
			{
				throw new TypeNotRegisteredException($"Type {abstractionType.Name} is not registered."); 
			}

			return registryItem;
		}

		private void AssertTypeNotAlreadyRegistered(Type abstractionType)
		{
			if (_registeredTypes.Any(x => x.AbstractionType == abstractionType))
			{
				throw new TypeAlreadyRegisteredException($"Type {abstractionType.Name} is already registered.");
			}
		}

		private void AssertTypeCanBeInstantiated(Type concreteType)
		{
			if (!concreteType.GetConstructors().Any())
			{
				throw new TypeCannotBeInstantiatedException($"Could not find any constructors for type {concreteType.Name}");
			}
		}
	}
}
