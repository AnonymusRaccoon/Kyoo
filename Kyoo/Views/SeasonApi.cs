using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kyoo.CommonApi;
using Kyoo.Controllers;
using Kyoo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Kyoo.Models.Options;
using Kyoo.Models.Permissions;
using Microsoft.Extensions.Options;

namespace Kyoo.Api
{
	[Route("api/season")]
	[Route("api/seasons")]
	[ApiController]
	[PartialPermission(nameof(SeasonApi))]
	public class SeasonApi : CrudApi<Season>
	{
		private readonly ILibraryManager _libraryManager;
		private readonly IThumbnailsManager _thumbs;
		private readonly IFileManager _files;

		public SeasonApi(ILibraryManager libraryManager,
			IOptions<BasicOptions> options,
			IThumbnailsManager thumbs,
			IFileManager files)
			: base(libraryManager.SeasonRepository, options.Value.PublicUrl)
		{
			_libraryManager = libraryManager;
			_thumbs = thumbs;
			_files = files;
		}
		
		[HttpGet("{seasonID:int}/episode")]
		[HttpGet("{seasonID:int}/episodes")]
		[PartialPermission(Kind.Read)]
		public async Task<ActionResult<Page<Episode>>> GetEpisode(int seasonID,
			[FromQuery] string sortBy,
			[FromQuery] int afterID,
			[FromQuery] Dictionary<string, string> where,
			[FromQuery] int limit = 30)
		{
			try
			{
				ICollection<Episode> resources = await _libraryManager.GetAll(
					ApiHelper.ParseWhere<Episode>(where, x => x.SeasonID == seasonID),
					new Sort<Episode>(sortBy),
					new Pagination(limit, afterID));

				if (!resources.Any() && await _libraryManager.GetOrDefault<Season>(seasonID) == null)
					return NotFound();
				return Page(resources, limit);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new {Error = ex.Message});
			}
		}
		
		[HttpGet("{showSlug}-s{seasonNumber:int}/episode")]
		[HttpGet("{showSlug}-s{seasonNumber:int}/episodes")]
		[PartialPermission(Kind.Read)]
		public async Task<ActionResult<Page<Episode>>> GetEpisode(string showSlug,
			int seasonNumber,
			[FromQuery] string sortBy,
			[FromQuery] int afterID,
			[FromQuery] Dictionary<string, string> where,
			[FromQuery] int limit = 30)
		{
			try
			{
				ICollection<Episode> resources = await _libraryManager.GetAll(
					ApiHelper.ParseWhere<Episode>(where, x => x.Show.Slug == showSlug 
					                                          && x.SeasonNumber == seasonNumber),
					new Sort<Episode>(sortBy),
					new Pagination(limit, afterID));

				if (!resources.Any() && await _libraryManager.GetOrDefault(showSlug, seasonNumber) == null)
					return NotFound();
				return Page(resources, limit);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new {Error = ex.Message});
			}
		}
		
		[HttpGet("{showID:int}-s{seasonNumber:int}/episode")]
		[HttpGet("{showID:int}-s{seasonNumber:int}/episodes")]
		[PartialPermission(Kind.Read)]
		public async Task<ActionResult<Page<Episode>>> GetEpisode(int showID,
			int seasonNumber,
			[FromQuery] string sortBy,
			[FromQuery] int afterID,
			[FromQuery] Dictionary<string, string> where,
			[FromQuery] int limit = 30)
		{
			try
			{
				ICollection<Episode> resources = await _libraryManager.GetAll(
					ApiHelper.ParseWhere<Episode>(where, x => x.ShowID == showID && x.SeasonNumber == seasonNumber),
					new Sort<Episode>(sortBy),
					new Pagination(limit, afterID));

				if (!resources.Any() && await _libraryManager.GetOrDefault(showID, seasonNumber) == null)
					return NotFound();
				return Page(resources, limit);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new {Error = ex.Message});
			}
		}
		
		[HttpGet("{seasonID:int}/show")]
		[PartialPermission(Kind.Read)]
		public async Task<ActionResult<Show>> GetShow(int seasonID)
		{
			Show ret = await _libraryManager.GetOrDefault<Show>(x => x.Seasons.Any(y => y.ID == seasonID));
			if (ret == null)
				return NotFound();
			return ret;
		}
		
		[HttpGet("{showSlug}-s{seasonNumber:int}/show")]
		[PartialPermission(Kind.Read)]
		public async Task<ActionResult<Show>> GetShow(string showSlug, int seasonNumber)
		{
			Show ret = await _libraryManager.GetOrDefault<Show>(showSlug);
			if (ret == null)
				return NotFound();
			return ret;
		}
		
		[HttpGet("{showID:int}-s{seasonNumber:int}/show")]
		[PartialPermission(Kind.Read)]
		public async Task<ActionResult<Show>> GetShow(int showID, int seasonNumber)
		{
			Show ret = await _libraryManager.GetOrDefault<Show>(showID);
			if (ret == null)
				return NotFound();
			return ret;
		}
		
		[HttpGet("{id:int}/thumb")]
		public async Task<IActionResult> GetThumb(int id)
		{
			Season season = await _libraryManager.GetOrDefault<Season>(id);
			if (season == null)
				return NotFound();
			await _libraryManager.Load(season, x => x.Show);
			return _files.FileResult(await _thumbs.GetSeasonPoster(season));
		}
		
		[HttpGet("{slug}/thumb")]
		public async Task<IActionResult> GetThumb(string slug)
		{
			Season season = await _libraryManager.GetOrDefault<Season>(slug);
			if (season == null)
				return NotFound();
			await _libraryManager.Load(season, x => x.Show);
			return _files.FileResult(await _thumbs.GetSeasonPoster(season));
		}
	}
}