using System;
using System.Collections.Generic;

namespace Kyoo.Models
{
	public interface IPluginMetadata
	{
		/// <summary>
		/// A slug to identify this plugin in queries.
		/// </summary>
		string Slug { get; }
		
		/// <summary>
		/// The name of the plugin
		/// </summary>
		string Name { get; }
		
		/// <summary>
		/// The description of this plugin. This will be displayed on the "installed plugins" page.
		/// </summary>
		string Description { get; }
		
		/// <summary>
		/// The URL used to download this plugin.
		/// </summary>
		string DownloadURL { get; }
		
		/// <summary>
		/// The version of this plugin.
		/// </summary>
		Version Version { get; }
		
		/// <summary>
		/// A list of services that are provided by this service. This allow other plugins to declare dependencies
		/// </summary>
		/// <remarks>
		/// You should put the type's interface that will be register in configure.
		/// </remarks>
		ICollection<Type> Provides { get; }
		
		/// <summary>
		/// A list of types that will be provided only if a condition is met. The condition can be an arbitrary method or
		/// a condition based on other type availability. For more information, see <see cref="ConditionalProvides"/>.
		/// </summary>
		ICollection<ConditionalProvide> ConditionalProvides { get; }

		/// <summary>
		/// A list of services that are required by this plugin.
		/// You can put services that you provide conditionally here if you want.
		/// Kyoo will warn the user that this plugin can't be loaded if a required service is not found.
		/// </summary>
		/// <remarks>
		/// Put here the most complete type that are needed for your plugin to work. If you need a LibraryManager,
		/// put typeof(ILibraryManager).
		/// </remarks>
		ICollection<Type> Requires { get; }
	}
}