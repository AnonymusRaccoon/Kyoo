namespace Kyoo.Models.DisplayableOptions
{
	/// <summary>
	/// A base class for displayable options.
	/// </summary>
	public class DisplayableOption
	{
		/// <summary>
		/// The type of this configuration.
		/// </summary>
		public enum OptionType
		{
			Number,
			String,
			Path
		}
		
		/// <summary>
		/// The slug to edit the configuration value with the ConfigurationAPI.
		/// </summary>
		public string Slug { get; init; }
		
		/// <summary>
		/// The name of the option
		/// </summary>
		public string Name { get; init; }
		
		/// <summary>
		/// The description of this option.
		/// </summary>
		public string Description { get; init; }
		
		/// <summary>
		/// An optional help message. It can be null.
		/// </summary>
		public string HelpMessage { get; init; }

		/// <summary>
		/// The type of this option.
		/// </summary>
		public OptionType Type { get; init; }
	}
}