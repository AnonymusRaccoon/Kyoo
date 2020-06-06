using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kyoo.Models;
using Kyoo.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Kyoo.Controllers
{
	public class LibraryRepository : ILibraryRepository
	{
		private readonly DatabaseContext _database;
		private readonly IProviderRepository _providers;


		public LibraryRepository(DatabaseContext database, IProviderRepository providers)
		{
			_database = database;
			_providers = providers;
		}
		
		public Task<Library> Get(long id)
		{
			return _database.Libraries.FirstOrDefaultAsync(x => x.ID == id);
		}
		
		public Task<Library> Get(string slug)
		{
			return _database.Libraries.FirstOrDefaultAsync(x => x.Name == slug);
		}

		public async Task<IEnumerable<Library>> Search(string query)
		{
			return await _database.Libraries
				.Where(x => EF.Functions.Like(x.Name, $"%{query}%"))
				.Take(20)
				.ToListAsync();
		}

		public async Task<IEnumerable<Library>> GetAll()
		{
			return await _database.Libraries.ToListAsync();
		}

		public async Task<long> Create(Library obj)
		{
			if (obj == null)
				throw new ArgumentNullException(nameof(obj));

			obj.Links = null;
			await Validate(obj);
			await _database.Libraries.AddAsync(obj);
			await _database.SaveChangesAsync();
			return obj.ID;
		}
		
		public async Task<long> CreateIfNotExists(Library obj)
		{
			if (obj == null)
				throw new ArgumentNullException(nameof(obj));

			Library old = await Get(obj.Name);
			if (old != null)
				return old.ID;
			return await Create(obj);
		}

		public async Task Edit(Library edited, bool resetOld)
		{
			if (edited == null)
				throw new ArgumentNullException(nameof(edited));
			
			Library old = await Get(edited.Name);

			if (old == null)
				throw new ItemNotFound($"No library found with the name {edited.Name}.");
			
			if (resetOld)
				Utility.Nullify(old);
			Utility.Merge(old, edited);
			await Validate(old);
			await _database.SaveChangesAsync();
		}

		private async Task Validate(Library obj)
		{
			obj.ProviderLinks = (await Task.WhenAll(obj.ProviderLinks.Select(async x =>
			{
				x.ProviderID = await _providers.CreateIfNotExists(x.Provider);
				return x;
			}))).ToList();
		}

		public async Task Delete(Library obj)
		{
			_database.Libraries.Remove(obj);
			await _database.SaveChangesAsync();
		}
	}
}