﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kyoo.Models
{
	public class People : IMergable<People>
	{
		public string Slug { get; set; }
		public string Name { get; set; }
		[JsonIgnore] public string ImgPrimary { get; set; }
		public string ExternalIDs { get; set; }
		
		[JsonIgnore] public virtual IEnumerable<PeopleLink> Roles { get; set; }
		
		public People() {}

		public People(string slug, string name, string imgPrimary, string externalIDs)
		{
			Slug = slug;
			Name = name;
			ImgPrimary = imgPrimary;
			ExternalIDs = externalIDs;
		}

		public People Merge(People other)
		{
			if (other == null)
				return this;
			Slug ??= other.Slug;
			Name ??= other.Name;
			ImgPrimary ??= other.ImgPrimary;
			ExternalIDs += '|' + other.ExternalIDs;
			return this;
		}
	}
}
