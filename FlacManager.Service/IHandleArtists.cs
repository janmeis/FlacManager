using FlacManager.Models;
using System;
using System.Collections.Generic;
using System.Text;
using FlacManager.Models.Models;

namespace FlacManager.Service
{
	public interface IHandleArtists
	{
		bool DeleteAll();
		void StoreArtists(DateTime? from, DateTime? to);
		IEnumerable<Artist> EnumerateArtists();
	}
}
