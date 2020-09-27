using FlacManager.Db.LiteDb;
using FlacManager.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlacManager.Service
{
	public class HandleArtists: IHandleArtists
	{
		private readonly ILiteDbMusicCatalogService _musicCalatalogDbService;
		private readonly string _musicFilesLocation;

		public HandleArtists(ILiteDbMusicCatalogService musicCalatalogDbService, IOptions<MusicLibraryOptions> options)
		{
			_musicCalatalogDbService = musicCalatalogDbService;
			_musicFilesLocation = options.Value.MusicFilesLocation;
		}

		public int DeleteCollection()
		{
			return _musicCalatalogDbService.DeleteCollection();
		}

		public void StoreArtists()
		{
			var artists = new List<Artist>();
			foreach (var artist in new DirectoryInfo(_musicFilesLocation).EnumerateDirectories().OrderBy(p => p.Name))
			{
				artists.Add(new Artist
				{
					Id = 0,
					Name = artist.Name,
					Albums = artist.EnumerateDirectories().Count(),
					Size = GetArtistDirSizeInMB(artist)
				});
			}

			artists.ForEach(a => _musicCalatalogDbService.Insert(a));
		}

		public IEnumerable<Artist> EnumerateArtists()
		{
			return _musicCalatalogDbService.FindAll();
		}


		private long GetArtistDirSizeInMB(DirectoryInfo artist)
		{
			return artist.EnumerateDirectories().Sum(d => GetAlbumDirSizeInMB(d))
				+ artist.EnumerateFiles().Sum(f => f.Length / 1024);
		}

		private long GetAlbumDirSizeInMB(DirectoryInfo album)
		{
			return album.EnumerateFiles().Sum(f => f.Length / 1024);
		}

	}
}
