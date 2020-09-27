using FlacManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlacManager.Service
{
	public interface IHandleArtists
	{
		int DeleteCollection();
		void StoreArtists();
		IEnumerable<Artist> EnumerateArtists();
	}
}
