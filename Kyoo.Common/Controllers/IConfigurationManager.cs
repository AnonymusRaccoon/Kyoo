using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Kyoo.Models;
using Kyoo.Models.DisplayableOptions;
using Kyoo.Models.Exceptions;

namespace Kyoo.Controllers
{
	/// <summary>
	/// A class to ease configuration management. This work WITH Microsoft's package, you can still use IOptions patterns
	/// to access your options, this manager ease dynamic work and editing.
	/// It works with <see cref="ConfigurationReference"/>.
	/// </summary>
	public interface IConfigurationManager
	{
		/// <summary>
		/// Add an editable configuration to the editable configuration list
		/// </summary>
		/// <param name="path">The root path of the editable configuration. It should not be a nested type.</param>
		/// <typeparam name="T">The type of the configuration</typeparam>
		void Register<T>([NotNull] string path);

		/// <summary>
		/// Add an editable configuration to the editable configuration list.
		/// WARNING: this method allow you to add an unmanaged type. This type won't be editable. This can be used
		/// for external libraries or variable arguments.
		/// </summary>
		/// <param name="path">The root path of the editable configuration. It should not be a nested type.</param>
		void RegisterUntyped([NotNull] string path);
		
		/// <summary>
		/// Get the value of a setting using it's path.
		/// </summary>
		/// <param name="path">The path of the resource (can be separated by ':' or '__')</param>
		/// <exception cref="ItemNotFoundException">No setting found at the given path.</exception>
		/// <returns>The value of the settings (if it's a strongly typed one, the given type is instantiated</returns>
		object GetValue([CanBeNull] string path);
		
		/// <summary>
		/// Get the value of a setting using it's path.
		/// If your don't need a strongly typed value, see <see cref="GetValue"/>.
		/// </summary>
		/// <param name="path">The path of the resource (can be separated by ':' or '__')</param>
		/// <typeparam name="T">A type to strongly type your option.</typeparam>
		/// <exception cref="ArgumentException">If your type is not the same as the registered type</exception>
		/// <exception cref="ItemNotFoundException">No setting found at the given path.</exception>
		/// <returns>The value of the settings (if it's a strongly typed one, the given type is instantiated</returns>
		T GetValue<T>([NotNull] string path);
		
		/// <summary>
		/// Edit the value of a setting using it's path. Save it to the json file.
		/// </summary>
		/// <param name="path">The path of the resource (can be separated by ':' or '__')</param>
		/// <param name="value">The new value of the resource</param>
		/// <exception cref="ArgumentException">If your type is not the same as the registered type</exception>
		/// <exception cref="ItemNotFoundException">No setting found at the given path.</exception>
		Task EditValue([NotNull] string path, object value);


		/// <summary>
		/// Add an editable panel to the list.
		/// </summary>
		/// <param name="sectionName">The name of the big section</param>
		/// <param name="options">The list of options to put on the section (this is ordered).</param>
		void RegisterPanel(string sectionName, ICollection<DisplayableOption> options);
		
		/// <summary>
		/// Get a dictionary of settings to display in a UI.
		/// </summary>
		/// <returns>A list of sections</returns>
		ICollection<ConfigurationSection> GetEditPanel();
	}
}