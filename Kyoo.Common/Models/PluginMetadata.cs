using System;

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
	}
}