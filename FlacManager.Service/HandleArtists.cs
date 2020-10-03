using FlacManager.Models.Interfaces;
using FlacManager.Models.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlacManager.Service
{
	public class HandleArtists : IHandleArtists
	{
		private readonly IMusicCatalogService _musicCatalogDbService;
		private readonly IAudioFileService _audioFileService;
		private readonly string _musicFilesLocation;
		//private static readonly Regex _trackRegex = new Regex(@"^(\d+)\s*-\s*(.*)$");

		public HandleArtists(
			IMusicCatalogService musicCatalogDbService,
			IAudioFileService audioFileService,
			IOptions<MusicLibraryOptions> options)
		{
			_musicCatalogDbService = musicCatalogDbService;
			_audioFileService = audioFileService;
			_musicFilesLocation = options.Value.MusicFilesLocation;
		}

		public bool DeleteAll()
		{
			return _musicCatalogDbService.DeleteAll();
		}

		public IEnumerable<Artist> EnumerateArtists()
		{
			return _musicCatalogDbService.FindAll();
		}

		public void StoreArtists(DateTime? from, DateTime? to)
		{
			var artists = new DirectoryInfo(_musicFilesLocation).EnumerateDirectories()
				.Where(artist => artist.CreationTime >= (from ?? DateTime.MinValue) && artist.CreationTime <= (to ?? DateTime.MaxValue))
				.OrderBy(artist => artist.Name)
				.Select(artist => new Artist
				{
					Id = 0,
					Name = artist.Name,
					Length = GetArtistDirSize(artist),
					CreationTime = artist.CreationTime,
					LastWriteTime = artist.LastWriteTime,
					Albums = GetAlbums(artist)
				}).ToList();

			//_musicCatalogDbService.CreateStatistics();
			_musicCatalogDbService.SetIndexes();
			artists.ForEach(a => _musicCatalogDbService.Insert(a));
		}

		private IEnumerable<Album> GetAlbums(DirectoryInfo artist)
		{
			var index = 1;
			return artist.EnumerateDirectories()
				.OrderBy(album => album.Name)
				.Select(album => new Album
				{
					Id = index++,
					Name = album.Name,
					Length = GetAlbumDirSize(album),
					Tracks = _audioFileService.GetTracks(album),
					CreationTime = album.CreationTime,
					LastWriteTime = album.LastWriteTime,
				});
		}

		private static long GetArtistDirSize(DirectoryInfo artist)
		{
			return (artist?.EnumerateDirectories().Sum(GetAlbumDirSize) ?? 0)
				   + (artist?.EnumerateFiles().Sum(f => f.Length) ?? 0);
		}

		private static long GetAlbumDirSize(DirectoryInfo album)
		{
			return album?.EnumerateFiles().Sum(f => f.Length) ?? 0;
		}
	}
}
