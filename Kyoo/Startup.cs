using System;
using System.IO;
using Kyoo.Authentication;
using Kyoo.Controllers;
using Kyoo.Models;
using Kyoo.Postgresql;
using Kyoo.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kyoo
{
	/// <summary>
	/// The Startup class is used to configure the AspNet's webhost.
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// A plugin manager used to load plugins and allow them to configure services / asp net.
		/// </summary>
		private readonly IPluginManager _plugins;


		/// <summary>
		/// Created from the DI container, those services are needed to load information and instantiate plugins.s
		/// </summary>
		/// <param name="plugins">The plugin manager to load plugins and allow them to configure the host.</param>
		public Startup(IPluginManager plugins)
		{
			_plugins = plugins;
			
			// TODO remove postgres and authentication from here and load it like a normal plugin.
			_plugins.LoadPlugins(
				typeof(CoreModule),
				typeof(PostgresModule),
				typeof(AuthenticationModule)
			);
		}

		/// <summary>
		/// Configure the WebApp services context.
		/// </summary>
		/// <param name="services">The service collection to fill.</param>
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().AddControllersAsServices();
			
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");
			});
			services.AddResponseCompression(x =>
			{
				x.EnableForHttps = true;
			});
			services.AddTransient(typeof(Lazy<>), typeof(LazyDi<>));
			services.AddTask<PluginInitializer>();
			_plugins.ConfigureServices(services);
		}
		
		/// <summary>
		/// Configure the asp net host.
		/// </summary>
		/// <param name="app">The asp net host to configure</param>
		/// <param name="env">The host environment (is the app in development mode?)</param>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			else
			{
				app.UseExceptionHandler("/error");
				app.UseHsts();
			}
			
			if (!env.IsDevelopment())
				app.UseSpaStaticFiles();

			app.UseRouting();
			app.Use((ctx, next) => 
			{
				ctx.Response.Headers.Remove("X-Powered-By");
				ctx.Response.Headers.Remove("Server");
				ctx.Response.Headers.Add("Feature-Policy", "autoplay 'self'; fullscreen");
				ctx.Response.Headers.Add("Content-Security-Policy", "default-src 'self' blob:; script-src 'self' blob: 'unsafe-inline' 'unsafe-eval'; style-src 'self' 'unsafe-inline'; frame-src 'self' https://www.youtube.com");
				ctx.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
				ctx.Response.Headers.Add("Referrer-Policy", "no-referrer");
				ctx.Response.Headers.Add("Access-Control-Allow-Origin", "null");
				ctx.Response.Headers.Add("X-Content-Type-Options", "nosniff");
				return next();
			});
			app.UseResponseCompression();
			
			_plugins.ConfigureAspnet(app);

			app.UseSpa(spa =>
			{
				spa.Options.SourcePath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Kyoo.WebApp");
			
				if (env.IsDevelopment())
					spa.UseAngularCliServer("start");
			});
		}
	}
}
