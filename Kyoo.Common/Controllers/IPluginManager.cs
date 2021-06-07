using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kyoo.Models;
using Kyoo.Models.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Kyoo.Controllers
{
	/// <summary>
	/// A manager to load plugins and retrieve information from them.
	/// </summary>
	public interface IPluginManager
	{
		/// <summary>
		/// Get a single plugin that match the type and name given.
		/// </summary>
		/// <param name="name">The name of the plugin</param>
		/// <typeparam name="T">The type of the plugin</typeparam>
		/// <exception cref="ItemNotFoundException">If no plugins match the query</exception>
		/// <returns>A plugin that match the queries</returns>
		public T GetPlugin<T>(string name);
		
		/// <summary>
		/// Get all plugins of the given type.
		/// </summary>
		/// <typeparam name="T">The type of plugins to get</typeparam>
		/// <returns>A list of plugins matching the given type or an empty list of none match.</returns>
		public ICollection<T> GetPlugins<T>();
		
		/// <summary>
		/// Get all plugins currently running on Kyoo. This also includes deleted plugins if the app as not been restarted.
		/// </summary>
		/// <returns>All plugins currently loaded.</returns>
		public ICollection<IPlugin> GetAllPlugins();
		
		/// <summary>
		/// List all plugin available on the installed repositories.
		/// </summary>
		/// <param name="query">A string to search a specific plugin.</param>
		/// <returns>The list of plugins available to download.</returns>
		public Task<ICollection<PluginRepository>> SearchRepositories(string query = null);

		/// <summary>
		/// Install the selected plugin. To enable it, you will need to restart Kyoo.
		/// </summary>
		/// <param name="plugin">The plugin to install</param>
		public Task Install(IPluginMetadata plugin);

		/// <summary>
		/// Uninstall a plugin by it's slug.
		/// </summary>
		/// <param name="pluginSlug">The slug of the plugin to uninstall</param>
		/// <exception cref="ItemNotFoundException">No plugin could be found with the specified slug.</exception>
		/// <exception cref="NotSupportedException">The specified plugin could not be removed.</exception>
		public void Uninstall(string pluginSlug);
		
		/// <summary>
		/// Uninstall a plugin.
		/// </summary>
		/// <param name="plugin">The plugin to uninstall.</param>
		/// <exception cref="NotSupportedException">The specified plugin could not be removed.</exception>
		public void Uninstall(IPlugin plugin);
		
		/// <summary>
		/// Load plugins and their dependencies from the plugin directory.
		/// </summary>
		/// <param name="plugins">
		/// An initial plugin list to use.
		/// You should not try to put plugins from the plugins directory here as they will get automatically loaded.
		/// </param>
		public void LoadPlugins(params IPlugin[] plugins);

		/// <summary>
		/// Load plugins and their dependencies from the plugin directory.
		/// </summary>
		/// <param name="plugins">
		/// An initial list of plugin type to use. Those plugins will be constructed by the PluginManager.
		/// You should not try to put plugins from the plugins directory here as they will get automatically loaded.
		/// </param>
		public void LoadPlugins(params Type[] plugins);
		
		/// <summary>
		/// Configure services adding or removing services as the plugins wants.
		/// </summary>
		/// <param name="services">The service collection to populate</param>
		public void ConfigureServices(IServiceCollection services);

		/// <summary>
		/// Configure an asp net application applying plugins policies.
		/// </summary>
		/// <param name="app">The asp net application to configure</param>
		/// <param name="provider">The service provider used to inject services to plugin's ConfigureAspNet</param>
		public void ConfigureAspnet(IApplicationBuilder app, IServiceProvider provider);
	}
}