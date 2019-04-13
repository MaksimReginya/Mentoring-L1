using System;

namespace IoCContainer.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ExportAttribute : Attribute
	{
		public ExportAttribute()
		{
		}

		public ExportAttribute(Type baseType)
		{
			this.BaseType = baseType;
		}

		public Type BaseType { get; private set; }
	}
}
