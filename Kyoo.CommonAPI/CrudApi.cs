﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kyoo.Controllers;
using Kyoo.Models;
using Kyoo.Models.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Kyoo.CommonApi
{
	[ApiController]
	[ResourceView]
	public class CrudApi<T> : ControllerBase where T : class, IResource
	{
		private readonly IRepository<T> _repository;
		protected readonly string BaseURL;

		public CrudApi(IRepository<T> repository, IConfiguration configuration)
		{
			_repository = repository;
			BaseURL = configuration.GetValue<string>("public_url").TrimEnd('/');
		}


		[HttpGet("{id:int}")]
		[Authorize(Policy = "Read")]
		public virtual async Task<ActionResult<T>> Get(int id)
		{
			T resource = await _repository.Get(id);
			if (resource == null)
				return NotFound();

			return resource;
		}

		[HttpGet("{slug}")]
		[Authorize(Policy = "Read")]
		public virtual async Task<ActionResult<T>> Get(string slug)
		{
			T resource = await _repository.Get(slug);
			if (resource == null)
				return NotFound();
			
			return resource;
		}

		[HttpGet("count")]
		[Authorize(Policy = "Read")]
		public virtual async Task<ActionResult<int>> GetCount([FromQuery] Dictionary<string, string> where)
		{
			try
			{
				return await _repository.GetCount(ApiHelper.ParseWhere<T>(where));
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new {Error = ex.Message});
			}
		}
		
		[HttpGet]
		[Authorize(Policy = "Read")]
		public virtual async Task<ActionResult<Page<T>>> GetAll([FromQuery] string sortBy,
			[FromQuery] int afterID,
			[FromQuery] Dictionary<string, string> where,
			[FromQuery] int limit = 20)
		{
			try
			{
				ICollection<T> resources = await _repository.GetAll(ApiHelper.ParseWhere<T>(where),
					new Sort<T>(sortBy),
					new Pagination(limit, afterID));

				return Page(resources, limit);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new {Error = ex.Message});
			}
		}

		protected Page<TResult> Page<TResult>(ICollection<TResult> resources, int limit)
			where TResult : IResource
		{
			return new(resources, 
				BaseURL + Request.Path,
				Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString(), StringComparer.InvariantCultureIgnoreCase),
				limit);
		}

		[HttpPost]
		[Authorize(Policy = "Write")]
		public virtual async Task<ActionResult<T>> Create([FromBody] T resource)
		{
			try
			{
				return await _repository.Create(resource);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new {Error = ex.Message});
			}
			catch (DuplicatedItemException)
			{
				T existing = await _repository.Get(resource.Slug);
				return Conflict(existing);
			}
		}
		
		[HttpPut]
		[Authorize(Policy = "Write")]
		public virtual async Task<ActionResult<T>> Edit([FromQuery] bool resetOld, [FromBody] T resource)
		{
			if (resource.ID > 0)
				return await _repository.Edit(resource, resetOld);
			
			T old = await _repository.Get(resource.Slug);
			if (old == null)
				return NotFound();
			
			resource.ID = old.ID;
			return await _repository.Edit(resource, resetOld);
		}

		[HttpPut("{id:int}")]
		[Authorize(Policy = "Write")]
		public virtual async Task<ActionResult<T>> Edit(int id, [FromQuery] bool resetOld, [FromBody] T resource)
		{
			resource.ID = id;
			try
			{
				return await _repository.Edit(resource, resetOld);
			}
			catch (ItemNotFound)
			{
				return NotFound();
			}
		}
		
		[HttpPut("{slug}")]
		[Authorize(Policy = "Write")]
		public virtual async Task<ActionResult<T>> Edit(string slug, [FromQuery] bool resetOld, [FromBody] T resource)
		{
			T old = await _repository.Get(slug);
			if (old == null)
				return NotFound();
			resource.ID = old.ID;
			return await _repository.Edit(resource, resetOld);
		}

		[HttpDelete("{id:int}")]
		[Authorize(Policy = "Write")]
		public virtual async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _repository.Delete(id);
			}
			catch (ItemNotFound)
			{
				return NotFound();
			}

			return Ok();
		}
		
		[HttpDelete("{slug}")]
		[Authorize(Policy = "Write")]
		public virtual async Task<IActionResult> Delete(string slug)
		{
			try
			{
				await _repository.Delete(slug);
			}
			catch (ItemNotFound)
			{
				return NotFound();
			}

			return Ok();
		}
	}
}