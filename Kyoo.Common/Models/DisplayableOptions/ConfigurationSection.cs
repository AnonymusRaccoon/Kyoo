using System.Collections.Generic;

namespace Kyoo.Models.DisplayableOptions
{
	/// <summary>
	/// A simple configuration section.
	/// </summary>
	public class ConfigurationSection
	{
		/// <summary>
		/// The name of the section.
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// The list of options in this section
		/// </summary>
		public ICollection<DisplayableOption> Options { get; set; }
	}
}