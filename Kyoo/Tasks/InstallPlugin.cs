using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kyoo.Controllers;
using Kyoo.Models;
using Kyoo.Models.Attributes;

namespace Kyoo.Tasks
{
	/// <summary>
	/// A task to install plugins.
	/// </summary>
	public class InstallPlugin : ITask
	{
		/// <inheritdoc />
		public string Slug => "install";

		/// <inheritdoc />
		public string Name => "Install Plugin";

		/// <inheritdoc />
		public string Description => "Install a plugin from an installed repository";

		/// <inheritdoc />
		public string HelpMessage => null;

		/// <inheritdoc />
		public bool RunOnStartup => false;

		/// <inheritdoc />
		public int Priority => 0;
		
		/// <summary>
		/// The plugin manager used to search plugins and install them.
		/// </summary>
		[Injected] public IPluginManager Plugins { private get; set; }
		
		/// <inheritdoc />
		public async Task Run(TaskParameters arguments, CancellationToken cancellationToken)
		{
			string slug = arguments["slug"].As<string>();
			ICollection<IPluginMetadata> plugins = (await Plugins.SearchRepositories(slug))
				.SelectMany(x => x.Plugins)
				.ToArray();
			if (!plugins.Any())
				throw new ArgumentException($"No plugin found with the slug: {slug}");
			await Plugins.Install(plugins.First());
		}

		/// <inheritdoc />
		public TaskParameters GetParameters()
		{
			return new (new [] {
				TaskParameter.Create<string>("slug", "The slug of the plugin to install.")
			});
		}

		/// <inheritdoc />
		public int? Progress()
		{
			return 0;
		}
	}
}