using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Kyoo.Models.Attributes;

namespace Kyoo
{
	public static class Helper
	{
		/// <summary>
		/// Inject services into the <see cref="InjectedAttribute"/> marked properties of the given object.
		/// </summary>
		/// <param name="obj">The object to inject</param>
		/// <param name="retrieve">The function used to retrieve services. (The function is called immediately)</param>
		public static void InjectServices(object obj, [InstantHandle] Func<Type, object> retrieve)
		{
			IEnumerable<PropertyInfo> properties = obj.GetType().GetProperties()
				.Where(x => x.GetCustomAttribute<InjectedAttribute>() != null)
				.Where(x => x.CanWrite);

			foreach (PropertyInfo property in properties)
				property.SetValue(obj, retrieve(property.PropertyType));
		}
	}
}