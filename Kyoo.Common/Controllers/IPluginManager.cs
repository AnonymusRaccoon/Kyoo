using System.Collections.Generic;
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
		/// Load plugins and their dependencies from the plugin directory.
		/// </summary>
		/// <param name="plugins">
		/// An initial plugin list to use.
		/// You should not try to put plugins from the plugins directory here as they will get automatically loaded.
		/// </param>
		public void LoadPlugins(ICollection<IPlugin> plugins);

		/// <summary>
		/// Configure services adding or removing services as the plugins wants.
		/// </summary>
		/// <param name="services">The service collection to populate</param>
		public void ConfigureServices(IServiceCollection services);
		
		/// <summary>
		/// Configure an asp net application applying plugins policies.
		/// </summary>
		/// <param name="app">The asp net application to configure</param>
		public void ConfigureAspnet(IApplicationBuilder app);
	}
}