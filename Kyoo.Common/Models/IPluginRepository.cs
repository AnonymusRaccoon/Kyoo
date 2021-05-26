using System.Collections.Generic;

namespace Kyoo.Models
{
	/// <summary>
	/// A class representing a plugin repository, a service that list plugins
	/// that you can use to download new ones or update yours.
	/// </summary>
	public class PluginRepository
	{
		/// <summary>
		/// The name of the repository.
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// A description of this repository.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// The list of plugins available.
		/// </summary>
		public ICollection<IPluginMetadata> Plugins { get; set; }
	}
}