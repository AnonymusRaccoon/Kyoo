using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualBasic.FileIO;

namespace Kyoo
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			if (args.Length > 0)
				FileSystem.CurrentDirectory = args[0];
			Console.WriteLine($"Running as {Environment.UserName} in {FileSystem.CurrentDirectory}.");
			await CreateWebHostBuilder(args).Build().RunAsync();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseKestrel(config => { config.AddServerHeader = false; })
				.UseUrls("http://*:5000")
				.UseStartup<Startup>();
	}
}
