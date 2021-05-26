using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Kyoo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Kyoo.Controllers
{
	/// <summary>
	/// A common interface used to discord plugins
	/// </summary>
	/// <remarks>You can inject services in the IPlugin constructor.
	/// You should only inject well known services like an ILogger, IConfiguration or IWebHostEnvironment.</remarks>
	[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
	public interface IPlugin : IPluginMetadata
	{
		/// <summary>
		/// A configure method that will be run on plugin's startup.
		/// </summary>
		/// <param name="services">A service container to register new services.</param>
		/// <param name="availableTypes">The list of types that are available for this instance. This can be used
		/// for conditional type. See <see cref="ProviderCondition.Has(System.Type,System.Collections.Generic.ICollection{System.Type})"/>
		/// or <see cref="ProviderCondition.Has(System.Collections.Generic.IEnumerable{System.Type},System.Collections.Generic.ICollection{System.Type})"/>>
		/// You can't simply check on the service collection because some dependencies might be registered after your plugin.
		/// </param>
		void Configure(IServiceCollection services, ICollection<Type> availableTypes);

		/// <summary>
		/// An optional configuration step to allow a plugin to change asp net configurations.
		/// WARNING: This is only called on Kyoo's startup so you must restart the app to apply this changes.
		/// </summary>
		/// <param name="app">The Asp.Net application builder. On most case it is not needed but you can use it to add asp net functionalities.</param>
		void ConfigureAspNet(IApplicationBuilder app) {}
		
		/// <summary>
		/// An optional function to execute and initialize your plugin.
		/// It can be used to initialize a database connection, fill initial data or anything.
		/// </summary>
		/// <param name="provider">A service provider to request services</param>
		void Initialize(IServiceProvider provider) {}
	}
}