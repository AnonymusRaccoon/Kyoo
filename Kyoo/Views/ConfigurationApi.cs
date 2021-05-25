using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Kyoo.Controllers;
using Kyoo.Models.DisplayableOptions;
using Kyoo.Models.Exceptions;
using Kyoo.Models.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Kyoo.Api
{
	/// <summary>
	/// An API to retrieve or edit configuration settings
	/// </summary>
	[Route("api/config")]
	[Route("api/configuration")]
	[ApiController]
	public class ConfigurationApi : Controller
	{
		/// <summary>
		/// The configuration manager used to retrieve and edit configuration values (while being type safe).
		/// </summary>
		private readonly IConfigurationManager _manager;

		/// <summary>
		/// Create a new <see cref="ConfigurationApi"/> using the given configuration.
		/// </summary>
		/// <param name="manager">The configuration manager used to retrieve and edit configuration values</param>
		public ConfigurationApi(IConfigurationManager manager)
		{
			_manager = manager;
		}

		/// <summary>
		/// Get all permissions.
		/// </summary>
		/// <returns>The associate list of values.</returns>
		/// <response code="200">Return the configuration values</response>
		[HttpGet]
		[Permission(nameof(ConfigurationApi), Kind.Read, Group.Admin)]
		public ActionResult<object> GetConfiguration()
		{
			return _manager.GetValue(null);
		}
		
		/// <summary>
		/// Edit a dictionary of permissions.
		/// </summary>
		/// <param name="newValues">The dictionary with the following format: {slug: value}</param>
		/// <returns>The dictionary of edited value.</returns>
		/// <response code="200">Return the dictionary of edited value. If a value could not be edited, it is removed from the dictionary</response>
		[HttpPut]
		[Permission(nameof(ConfigurationApi), Kind.Write, Group.Admin)]
		[SuppressMessage("ReSharper", "RedundantJumpStatement")]
		public async Task<Dictionary<string, object>> EditConfiguration([FromBody] Dictionary<string, object> newValues)
		{
			Dictionary<string, object> ret = new();
			foreach ((string slug, object value) in newValues)
			{
				try
				{
					await _manager.EditValue(slug, value);
					ret[slug] = value;
				}
				catch
				{
					continue;
				}
			}
			return ret;
		}
		
		/// <summary>
		/// Get a permission from it's slug.
		/// </summary>
		/// <param name="slug">The permission to retrieve. You can use ':' or "__" to get a child value.</param>
		/// <returns>The associate value or list of values.</returns>
		/// <response code="200">Return the configuration value or the list of configurations</response>
		/// <response code="404">No configuration exists for the given slug</response>
		[HttpGet("{slug}")]
		[Permission(nameof(ConfigurationApi), Kind.Read, Group.Admin)]
		public ActionResult<object> GetConfiguration(string slug)
		{
			try
			{
				return _manager.GetValue(slug);
			}
			catch (ItemNotFoundException)
			{
				return NotFound();
			}
		}

		/// <summary>
		/// Edit a permission from it's slug.
		/// </summary>
		/// <param name="slug">The permission to edit. You can use ':' or "__" to get a child value.</param>
		/// <param name="newValue">The new value of the configuration</param>
		/// <returns>The edited value.</returns>
		/// <response code="200">Return the edited value</response>
		/// <response code="404">No configuration exists for the given slug</response>
		[HttpPut("{slug}")]
		[Permission(nameof(ConfigurationApi), Kind.Write, Group.Admin)]
		public async Task<ActionResult<object>> EditConfiguration(string slug, [FromBody] object newValue)
		{
			try
			{
				await _manager.EditValue(slug, newValue);
				return newValue;
			}
			catch (ItemNotFoundException)
			{
				return NotFound();
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
		}
		
		/// <summary>
		/// Get the permission UI.
		/// </summary>
		/// <returns>The permission UI sections.</returns>
		/// <response code="200">Return The permission UI sections</response>
		[HttpGet("panel")]
		[Permission(nameof(ConfigurationApi), Kind.Read, Group.Admin)]
		public ICollection<ConfigurationSection> GetPanel()
		{
			return _manager.GetEditPanel();
		}
	}
}