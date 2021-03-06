﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Kyoo.Controllers;
using Kyoo.Models.Exceptions;
using Kyoo.Models.Permissions;

namespace Kyoo.Api
{
	[Route("api/task")]
	[Route("api/tasks")]
	[ApiController]
	public class TaskApi : ControllerBase
	{
		private readonly ITaskManager _taskManager;

		public TaskApi(ITaskManager taskManager)
		{
			_taskManager = taskManager;
		}


		[HttpGet]
		[Permission(nameof(TaskApi), Kind.Read)]
		public ActionResult<ICollection<ITask>> GetTasks()
		{
			return Ok(_taskManager.GetAllTasks());
		}
		
		[HttpGet("{taskSlug}")]
		[HttpPut("{taskSlug}")]
		[Permission(nameof(TaskApi), Kind.Create)]
		public IActionResult RunTask(string taskSlug, [FromQuery] Dictionary<string, object> args)
		{
			try
			{
				_taskManager.StartTask(taskSlug, args);
				return Ok();
			}
			catch (ItemNotFoundException)
			{
				return NotFound();
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new {Error = ex.Message});
			}
		}
	}
}
