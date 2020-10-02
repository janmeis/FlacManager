using System;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FlacManager.Models.Interfaces;
using FlacManager.Models.Models;
using File = FlacManager.Models.Models.File;

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
			var startDate = from ?? DateTime.MinValue;
			var endDate = to ?? DateTime.MaxValue;

			var artists = new DirectoryInfo(_musicFilesLocation).EnumerateDirectories()
				.Where(artist => artist.CreationTime >= startDate && artist.CreationTime <= endDate)
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
			var indexSet = false;
			artists.ForEach(a =>
			{
				_musicCatalogDbService.Insert(a);
				if (!indexSet)
				{
					_musicCatalogDbService.SetIndexes();
					indexSet = true;
				}
			});
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
					Files = GetFiles(album)
				});
		}

		private static IEnumerable<File> GetFiles(DirectoryInfo album)
		{
			var index = 1;
			return album.EnumerateFileSystemInfos()
				.OrderBy(file => file.Name)
				.Select(file => new File
				{
					Id = index++,
					Name = file.Name,
					Length = (file.Attributes & FileAttributes.Directory) == 0 ? ((FileInfo) file).Length : 0,
					Attributes = file.Attributes,
					FullName = file.FullName,
					IsReadOnly = (file.Attributes & FileAttributes.Directory) == 0 && ((FileInfo) file).IsReadOnly,
					CreationTime = file.CreationTime,
					LastWriteTime = file.LastWriteTime
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
