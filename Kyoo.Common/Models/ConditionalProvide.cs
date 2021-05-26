using System;
using System.Collections.Generic;
using System.Linq;

namespace Kyoo.Models
{
	/// <summary>
	/// A type that will only be provided if a special condition is met. To check that your condition is met,
	/// you can check the <see cref="ProviderCondition"/> class.
	/// </summary>
	public class ConditionalProvide : Tuple<Type, ProviderCondition>
	{
		/// <summary>
		/// Get the type that may be provided
		/// </summary>
		public Type Type => Item1;

		/// <summary>
		/// Get the condition.
		/// </summary>
		public ProviderCondition Condition => Item2;
		
		/// <summary>
		/// Create a <see cref="ConditionalProvide"/> from a type and a condition.
		/// </summary>
		/// <param name="type">The type to provide</param>
		/// <param name="condition">The condition</param>
		public ConditionalProvide(Type type, ProviderCondition condition) 
			: base(type, condition) 
		{ }

		/// <summary>
		/// Create a <see cref="ConditionalProvide"/> from a tuple of (Type, ProviderCondition).
		/// </summary>
		/// <param name="tuple">The tuple to convert</param>
		public ConditionalProvide((Type type, ProviderCondition condition) tuple)
			: base(tuple.type, tuple.condition)
		{ }

		/// <summary>
		/// Implicitly convert a tuple to a <see cref="ConditionalProvide"/>.
		/// </summary>
		/// <param name="tuple">The tuple to convert</param>
		/// <returns>A new <see cref="ConditionalProvide"/> based on the given tuple.</returns>
		public static implicit operator ConditionalProvide((Type, Type) tuple) => new (tuple);
	}
	
	/// <summary>
	/// A condition for a conditional type.
	/// </summary>
	public class ProviderCondition
	{
		/// <summary>
		/// The condition as a method. If true is returned, the type will be provided.
		/// </summary>
		public Func<bool> Condition { get; } = () => true;  
		/// <summary>
		/// The list of types that this method needs.
		/// </summary>
		public ICollection<Type> Needed { get; } = ArraySegment<Type>.Empty;

		
		/// <summary>
		/// Create a new <see cref="ProviderCondition"/> from a raw function.
		/// </summary>
		/// <param name="condition">The predicate that will be used as condition</param>
		public ProviderCondition(Func<bool> condition)
		{
			Condition = condition;
		}
		
		/// <summary>
		/// Create a new <see cref="ProviderCondition"/> from a type. This allow you to inform that a type will
		/// only be available if a dependency is met.
		/// </summary>
		/// <param name="needed">The type that you need</param>
		public ProviderCondition(Type needed)
		{
			Needed = new[] {needed};
		}

		/// <summary>
		/// Create a new <see cref="ProviderCondition"/> from a list of type. This allow you to inform that a type will
		/// only be available if a list of dependencies are met.
		/// </summary>
		/// <param name="needed">The types that you need</param>
		public ProviderCondition(ICollection<Type> needed)
		{
			Needed = needed;
		}

		/// <summary>
		/// Create a new <see cref="ProviderCondition"/> with a list of types as dependencies and a predicate
		/// for arbitrary conditions.
		/// </summary>
		/// <param name="needed">The list of dependencies</param>
		/// <param name="condition">An arbitrary condition</param>
		public ProviderCondition(ICollection<Type> needed, Func<bool> condition)
		{
			Needed = needed;
			Condition = condition;
		}

		
		/// <summary>
		/// Implicitly convert a type to a <see cref="ProviderCondition"/>. 
		/// </summary>
		/// <param name="type">The type dependency</param>
		/// <returns>A <see cref="ProviderCondition"/> that will return true if the given type is available.</returns>
		public static implicit operator ProviderCondition(Type type) => new(type);
		
		/// <summary>
		/// Implicitly convert a list of type to a <see cref="ProviderCondition"/>. 
		/// </summary>
		/// <param name="types">The list of type dependencies</param>
		/// <returns>A <see cref="ProviderCondition"/> that will return true if the given types are available.</returns>
		public static implicit operator ProviderCondition(Type[] types) => new(types);
		
		/// <inheritdoc cref="op_Implicit(System.Type[])"/>
		public static implicit operator ProviderCondition(List<Type> types) => new(types);
		
		
		/// <summary>
		/// Check if a type is available.
		/// </summary>
		/// <param name="needed">The type to check</param>
		/// <param name="available">The list of types</param>
		/// <returns>True if the dependency is met, false otherwise</returns>
		public static bool Has(Type needed, ICollection<Type> available)
		{
			return available.Contains(needed);
		}
		
		/// <summary>
		/// Check if a list of type are available.
		/// </summary>
		/// <param name="needed">The list of types to check</param>
		/// <param name="available">The list of types</param>
		/// <returns>True if the dependencies are met, false otherwise</returns>
		public static bool Has(IEnumerable<Type> needed, ICollection<Type> available)
		{
			return needed.All(x => Has(x, available));
		}
	}
}