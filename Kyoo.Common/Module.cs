using System;
using Kyoo.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kyoo
{
	/// <summary>
	/// A static class with helper functions to setup external modules
	/// </summary>
	public static class Module
	{
		/// <summary>
		/// Register a new task to the container.
		/// </summary>
		/// <param name="services">The container</param>
		/// <typeparam name="T">The type of the task</typeparam>
		/// <returns>The initial container.</returns>
		public static IServiceCollection AddTask<T>(this IServiceCollection services)
			where T : class, ITask
		{
			services.AddSingleton<ITask, T>();
			return services;
		}

		/// <summary>
		/// Register a new repository to the container.
		/// </summary>
		/// <param name="services">The container</param>
		/// <param name="lifetime">The lifetime of the repository. The default is scoped.</param>
		/// <typeparam name="T">The type of the repository.</typeparam>
		/// <remarks>
		/// If your repository implements a special interface, please use <see cref="AddRepository{T,T2}"/>
		/// </remarks>
		/// <returns>The initial container.</returns>
		public static IServiceCollection AddRepository<T>(this IServiceCollection services, 
			ServiceLifetime lifetime = ServiceLifetime.Scoped)
			where T : IBaseRepository
		{
			Type repository = Utility.GetGenericDefinition(typeof(T), typeof(IRepository<>));

			if (repository != null)
				services.Add(ServiceDescriptor.Describe(repository, typeof(T), lifetime));
			services.Add(ServiceDescriptor.Describe(typeof(IBaseRepository), typeof(T), lifetime));
			return services;
		}

		/// <summary>
		/// Register a new repository with a custom mapping to the container.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="lifetime">The lifetime of the repository. The default is scoped.</param>
		/// <typeparam name="T">The custom mapping you have for your repository.</typeparam>
		/// <typeparam name="T2">The type of the repository.</typeparam>
		/// <remarks>
		/// If your repository does not implements a special interface, please use <see cref="AddRepository{T}"/>
		/// </remarks>
		/// <returns>The initial container.</returns>
		public static IServiceCollection AddRepository<T, T2>(this IServiceCollection services,
			ServiceLifetime lifetime = ServiceLifetime.Scoped)
			where T2 : IBaseRepository, T
		{
			services.Add(ServiceDescriptor.Describe(typeof(T), typeof(T2), lifetime));
			return services.AddRepository<T2>(lifetime);
		}

		/// <summary>
		/// Get the public URL of kyoo using the given configuration instance.
		/// </summary>
		/// <param name="configuration">The configuration instance</param>
		/// <returns>The public URl of kyoo (without a slash at the end)</returns>
		public static string GetPublicUrl(this IConfiguration configuration)
		{
			return configuration["basics:publicUrl"]?.TrimEnd('/') ?? "http://localhost:5000";
		}
	}
}