using System;

namespace Fte.Ioc.Registry
{
	internal class TypeRegistryItem
	{
		public Type AbstractionType { get; private set; }
		public Type ConcreteType { get; private set; }
		public LifeCycle LifeCycle { get; private set; }


		public TypeRegistryItem(Type abstractionType, Type concreteType, LifeCycle lifeCycle)
		{
			if (abstractionType == null) throw new ArgumentNullException(nameof(abstractionType));
			if (concreteType == null) throw new ArgumentNullException(nameof(concreteType));

			AbstractionType = abstractionType;
			ConcreteType = concreteType;
			LifeCycle = lifeCycle;
		}
	}
}