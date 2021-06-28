using System;
using System.Collections.Generic;
using Kyoo.Controllers;
using Kyoo.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ExamplePlugin
{
	public class Example : IPlugin
	{
		public string Slug => "example";
		public string Name => "Example";
		public string Description => "An example plugin that prints";
		public string DownloadURL => "";
		public Version Version => new(1, 0, 0, 0);
		public ICollection<Type> Provides => ArraySegment<Type>.Empty;
		public ICollection<ConditionalProvide> ConditionalProvides => ArraySegment<ConditionalProvide>.Empty;
		public ICollection<Type> Requires => ArraySegment<Type>.Empty;
		public void Configure(IServiceCollection services, ICollection<Type> availableTypes)
		{
			Console.WriteLine("Hello from a plugin.");
		}
	}
}