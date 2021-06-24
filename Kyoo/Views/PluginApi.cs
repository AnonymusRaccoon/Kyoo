using System.Collections.Generic;
using System.Threading.Tasks;
using Kyoo.Controllers;
using Kyoo.Models;
using Kyoo.Models.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Kyoo.Api
{
	[Route("api/plugin")]
	[Route("api/plugins")]
	[ApiController]
	public class PluginApi : ControllerBase
	{
		private readonly IPluginManager _pluginManager;
		
		public PluginApi(IPluginManager pluginManager)
		{
			_pluginManager = pluginManager;
		}

		[HttpGet("{query?}")]
		[Permission("plugin", Kind.Read, Group.Admin)]
		public async Task<ICollection<PluginRepository>> SearchPlugins(string query = null)
		{
			return await _pluginManager.SearchRepositories(query);
		}
		
		
		[HttpPut]
		[Permission("plugin", Kind.Read, Group.Admin)]
		public async Task<IActionResult> InstallPlugin([FromBody] PluginMetadata plugin)
		{
			await _pluginManager.Install(plugin);
			return Ok();
		}
	}
}