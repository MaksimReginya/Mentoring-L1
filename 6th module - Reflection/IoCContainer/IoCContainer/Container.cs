using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IoCContainer.Attributes;

namespace IoCContainer
{
	public class Container
	{
		private readonly IDictionary<Type, Type> _registeredTypes;

		public Container()
		{
			_registeredTypes = new Dictionary<Type, Type>();
		}

		public void AddType(Type type, Type baseType)
		{
			_registeredTypes.Add(baseType, type);
		}

		public void AddType(Type type)
		{
			this.AddType(type, type);
		}

		public void AddAssembly(Assembly assembly)
		{
			IEnumerable<Type> publicTypes = assembly.ExportedTypes;
			foreach (Type type in publicTypes)
			{
				ImportConstructorAttribute importConstructorAttribute = type.GetCustomAttribute<ImportConstructorAttribute>();
				bool hasImportedProperties = this.GetPropertiesWithImportAttribute(type).Any();
				if (importConstructorAttribute != null && hasImportedProperties)
				{
					throw new IoCContainerException("Class cannot contain ImportConstructorAttribute and ImportAttribute at the same time");
				}
				else if (importConstructorAttribute != null ^ hasImportedProperties)
				{
					this.AddType(type);
					continue;
				}

				IEnumerable<ExportAttribute> exportAttributes = type.GetCustomAttributes<ExportAttribute>();
				foreach (ExportAttribute exportAttribute in exportAttributes)
				{
					this.AddType(type, exportAttribute.BaseType ?? type);
				}
			}
		}

		public object CreateInstance(Type type)
		{
			return this.BuildInstance(type);
		}

		public T CreateInstance<T>()
		{
			return (T)this.CreateInstance(typeof(T));
		}

		private object BuildInstance(Type keyType)
		{
			if (!_registeredTypes.ContainsKey(keyType))
			{
				throw new IoCContainerException($"Dependency of type {keyType.FullName} has not been registered");
			}

			Type type = _registeredTypes[keyType];
			ConstructorInfo constructorInfo = this.GetConstructorInfo(type);
			object instance = this.BuildInstance(type, constructorInfo);

			if (type.GetCustomAttribute<ImportConstructorAttribute>() != null)
			{
				return instance;
			}

			this.InstantiateProperties(type, instance);
			return instance;
		}

		private void InstantiateProperties(Type type, object instance)
		{
			IEnumerable<PropertyInfo> propertiesInfo = this.GetPropertiesWithImportAttribute(type);
			foreach (PropertyInfo propertyInfo in propertiesInfo)
			{
				object instantiatedProperty = this.BuildInstance(propertyInfo.PropertyType);
				propertyInfo.SetValue(instance, instantiatedProperty);
			}
		}

		private object BuildInstance(Type type, ConstructorInfo constructorInfo)
		{
			List<object> parametersInstances = new List<object>();
			foreach (ParameterInfo constructorParameter in constructorInfo.GetParameters())
			{
				object parameterInstance = this.BuildInstance(constructorParameter.ParameterType);
				parametersInstances.Add(parameterInstance);
			}

			return Activator.CreateInstance(type, parametersInstances.ToArray());
		}

		private ConstructorInfo GetConstructorInfo(Type type)
		{
			ConstructorInfo[] constructors = type.GetConstructors();

			if (constructors.Length != 1)
			{
				throw new IoCContainerException($"Instantiated type {type.FullName} must have one public constructor");
			}

			return constructors[0];
		}

		private IEnumerable<PropertyInfo> GetPropertiesWithImportAttribute(Type type)
		{
			return type.GetProperties()
				.Where(property => property.GetCustomAttribute<ImportAttribute>() != null);
		}
	}
}
