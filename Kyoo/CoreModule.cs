using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Kyoo.Controllers;
using Kyoo.Models;
using Kyoo.Models.Attributes;
using Kyoo.Models.DisplayableOptions;
using Kyoo.Models.Options;
using Kyoo.Models.Permissions;
using Kyoo.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Kyoo
{
	/// <summary>
	/// The core module containing default implementations
	/// </summary>
	public class CoreModule : IPlugin
	{
		/// <inheritdoc />
		public string Slug => "core";
		
		/// <inheritdoc />
		public string Name => "Core";
		
		/// <inheritdoc />
		public string Description => "The core module containing default implementations.";

		/// <inheritdoc />
		public string DownloadURL => null;

		/// <inheritdoc />
		public Version Version => Assembly.GetExecutingAssembly().GetName().Version;

		/// <inheritdoc />
		public ICollection<Type> Provides => new[]
		{
			typeof(IFileManager),
			typeof(ITranscoder),
			typeof(IThumbnailsManager),
			typeof(IProviderManager),
			typeof(ITaskManager),
			typeof(ILibraryManager),
			typeof(IConfigurationManager)
		};

		/// <inheritdoc />
		public ICollection<ConditionalProvide> ConditionalProvides => new ConditionalProvide[]
		{
			(typeof(ILibraryRepository), typeof(DatabaseContext)),
			(typeof(ILibraryItemRepository), typeof(DatabaseContext)),
			(typeof(ICollectionRepository), typeof(DatabaseContext)),
			(typeof(IShowRepository), typeof(DatabaseContext)),
			(typeof(ISeasonRepository), typeof(DatabaseContext)),
			(typeof(IEpisodeRepository), typeof(DatabaseContext)),
			(typeof(ITrackRepository), typeof(DatabaseContext)),
			(typeof(IPeopleRepository), typeof(DatabaseContext)),
			(typeof(IStudioRepository), typeof(DatabaseContext)),
			(typeof(IGenreRepository), typeof(DatabaseContext)),
			(typeof(IProviderRepository), typeof(DatabaseContext)),
			(typeof(IUserRepository), typeof(DatabaseContext))
		};

		/// <inheritdoc />
		public ICollection<Type> Requires => new []
		{
			typeof(ILibraryRepository),
			typeof(ILibraryItemRepository),
			typeof(ICollectionRepository),
			typeof(IShowRepository),
			typeof(ISeasonRepository),
			typeof(IEpisodeRepository),
			typeof(ITrackRepository),
			typeof(IPeopleRepository),
			typeof(IStudioRepository),
			typeof(IGenreRepository),
			typeof(IProviderRepository)
		};

		
		/// <summary>
		/// The configuration to use.
		/// </summary>
		private readonly IConfiguration _configuration;
		
		/// <summary>
		/// The configuration manager used to specify type of configuration sections.
		/// </summary>
		[Injected] public IConfigurationManager ConfigurationManager { private get; set; }

		
		/// <summary>
		/// Create a new core module instance and use the given configuration.
		/// </summary>
		/// <param name="configuration">The configuration to use</param>
		public CoreModule(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		/// <inheritdoc />
        public void Configure(IServiceCollection services, ICollection<Type> availableTypes)
		{
			string publicUrl = _configuration.GetPublicUrl();

			services.Configure<BasicOptions>(_configuration.GetSection(BasicOptions.Path));
			services.Configure<TaskOptions>(_configuration.GetSection(TaskOptions.Path));
			services.Configure<MediaOptions>(_configuration.GetSection(MediaOptions.Path));

			services.AddControllers()
				.AddNewtonsoftJson(x =>
				{
					x.SerializerSettings.ContractResolver = new JsonPropertyIgnorer(publicUrl);
					x.SerializerSettings.Converters.Add(new PeopleRoleConverter());
				});
			
			services.AddSingleton<IConfigurationManager, ConfigurationManager>();
			services.AddSingleton<IFileManager, FileManager>();
			services.AddSingleton<ITranscoder, Transcoder>();
			services.AddSingleton<IThumbnailsManager, ThumbnailsManager>();
			services.AddSingleton<IProviderManager, ProviderManager>();
			services.AddSingleton<ITaskManager, TaskManager>();
			services.AddHostedService(x => x.GetService<ITaskManager>() as TaskManager);
			
			services.AddScoped<ILibraryManager, LibraryManager>();

			if (ProviderCondition.Has(typeof(DatabaseContext), availableTypes))
			{
				services.AddRepository<ILibraryRepository, LibraryRepository>();
				services.AddRepository<ILibraryItemRepository, LibraryItemRepository>();
				services.AddRepository<ICollectionRepository, CollectionRepository>();
				services.AddRepository<IShowRepository, ShowRepository>();
				services.AddRepository<ISeasonRepository, SeasonRepository>();
				services.AddRepository<IEpisodeRepository, EpisodeRepository>();
				services.AddRepository<ITrackRepository, TrackRepository>();
				services.AddRepository<IPeopleRepository, PeopleRepository>();
				services.AddRepository<IStudioRepository, StudioRepository>();
				services.AddRepository<IGenreRepository, GenreRepository>();
				services.AddRepository<IProviderRepository, ProviderRepository>();
				services.AddRepository<IUserRepository, UserRepository>();
			}

			services.AddTask<Crawler>();

			if (services.All(x => x.ServiceType != typeof(IPermissionValidator)))
				services.AddSingleton<IPermissionValidator, PassthroughPermissionValidator>();
		}

		/// <inheritdoc />
		public void ConfigureAspNet(IApplicationBuilder app)
		{
			ConfigurationManager.Register<BasicOptions>(BasicOptions.Path);
			ConfigurationManager.Register<TaskOptions>(TaskOptions.Path);
			ConfigurationManager.Register<MediaOptions>(MediaOptions.Path);
			ConfigurationManager.RegisterUntyped("database");
			ConfigurationManager.RegisterUntyped("logging");
			SetupEditableOptions();

			FileExtensionContentTypeProvider contentTypeProvider = new();
			contentTypeProvider.Mappings[".data"] = "application/octet-stream";
			app.UseStaticFiles(new StaticFileOptions
			{
				ContentTypeProvider = contentTypeProvider,
				FileProvider = new PhysicalFileProvider(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "wwwroot"))
			});
			
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		/// <summary>
		/// Register editable panels to configure the core app.
		/// </summary>
		private void SetupEditableOptions()
		{
			ConfigurationManager.RegisterPanel("Urls", new []
			{
				new DisplayableOption
				{
					Slug = "basics:url",
					Type = DisplayableOption.OptionType.String,
					Name = "Bind URL",
					Description = "The URL(s) that kyoo will listen to. This allow you to specify a port, a protocol and an host..",
					HelpMessage = "The default is \"http://*:5000\". The * means any bindable address.",
				},
				new DisplayableOption
				{
					Slug = "basics:publicUrl",
					Type = DisplayableOption.OptionType.String,
					Name = "Public URL",
					Description = "The public url to access kyoo. This will be the only address allowed for authentication.",
					HelpMessage = "If you want to use https, you must register an ssl certificate and setup a reverse proxy yourself.",
				}
			});
			ConfigurationManager.RegisterPanel("Paths", new []
			{
				new DisplayableOption
				{
					Slug = "basics:pluginsPath",
					Type = DisplayableOption.OptionType.Path,
					Name = "Plugin Directory",
					Description = "The path where you will store your plugins. This is relative to your installation path.",
					HelpMessage = $"Your installation path is: {Environment.CurrentDirectory}",
				},
				new DisplayableOption
				{
					Slug = "basics:peoplePath",
					Type = DisplayableOption.OptionType.Path,
					Name = "People Directory",
					Description = "The path where people pictures will be saved. This is relative to your installation path.",
					HelpMessage = $"Your installation path is: {Environment.CurrentDirectory}",
				},
				new DisplayableOption
				{
					Slug = "basics:providerPath",
					Type = DisplayableOption.OptionType.Path,
					Name = "Provider Directory",
					Description = "The path where provider icons will be cached. This is relative to your installation path.",
					HelpMessage = $"Your installation path is: {Environment.CurrentDirectory}",
				},
				new DisplayableOption
				{
					Slug = "basics:transmuxPath",
					Type = DisplayableOption.OptionType.Path,
					Name = "Transmux Cache Directory",
					Description = "The path where transmuxed videos will be stored. This is relative to your installation path.",
					HelpMessage = $"Your installation path is: {Environment.CurrentDirectory}",
				},
				new DisplayableOption
				{
					Slug = "basics:transcodePath",
					Type = DisplayableOption.OptionType.Path,
					Name = "Transcode Cache Directory",
					Description = "The path where transcoded videos will be stored. This is relative to your installation path.",
					HelpMessage = $"Your installation path is: {Environment.CurrentDirectory}",
				}
			});
		}
	}
}