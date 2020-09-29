using System;
using FlacManager.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FlacManager.Models.Interfaces;
using FlacManager.Models.Models;

namespace FlacManager.Service
{
	public class HandleArtists : IHandleArtists
	{
		private readonly IMusicCatalogService _musicCatalogDbService;
		private readonly string _musicFilesLocation;
		private static readonly Regex _albumRegex = new Regex(@"^\[([^\]]*)\]\s*(.*)$");
		private static readonly Regex _trackRegex = new Regex(@"^(\d+)\s*-\s*(.*)$");
		private static readonly Regex _extensionsRegex = new Regex(@"^(\.flac)|(\.mp3)|(\.m4a)|(\.ape)$", RegexOptions.IgnoreCase);

		public HandleArtists(IMusicCatalogService musicCatalogDbService, IOptions<MusicLibraryOptions> options)
		{
			_musicCatalogDbService = musicCatalogDbService;
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

		public void StoreArtists()
		{
			var artists = new DirectoryInfo(_musicFilesLocation).EnumerateDirectories()
				.OrderBy(artist => artist.Name)
				.Select(artist => new Artist
				{
					Id = 0,
					Name = artist.Name,
					Size = GetArtistDirSizeInMb(artist),
					Albums = GetAlbums(artist)
				}).ToList();

			artists.ForEach(a => _musicCatalogDbService.Insert(a));
		}

		private static IEnumerable<Album> GetAlbums(DirectoryInfo artist)
		{
			var index = 1;
			return artist.EnumerateDirectories()
				.OrderBy(album => album.Name)
				.Select(album => new Album
				{
					Id = index++,
					Name = GetAlbumName(album),
					Size = GetAlbumDirSizeInMb(album),
					Year = GetAlbumYear(album),
					Tracks = GetTracks(album)
				});
		}

		private static IEnumerable<Track> GetTracks(DirectoryInfo album)
		{
			var index = 1;
			return album.EnumerateFiles()
				.Where(f => _extensionsRegex.IsMatch(f.Extension))
				.OrderBy(track => track.Name)
				.Select(track => new Track
				{
					Id = index++,
					Name = GetTrackName(track),
					Size = GetTrackSizeInMb(track),
					Number = GetTrackNumber(track),
					Format = track.Extension.Replace(".", "")
				});
		}

		private static string GetTrackName(FileSystemInfo track) => GetTrackSomething(Path.GetFileNameWithoutExtension(track.FullName), 1);

		private static int GetTrackNumber(FileSystemInfo track)
		{
			var number = GetTrackSomething(track.Name, 0);
			return string.IsNullOrEmpty(number) ? 0 : int.Parse(number);
		}

		private static string GetTrackSomething(string trackName, int index) =>
			_trackRegex.IsMatch(trackName)
				? _trackRegex.Split(trackName).Where(a => !string.IsNullOrWhiteSpace(a)).ToArray()[index]
				: index == 0 ? string.Empty : trackName;

		private static string GetAlbumName(FileSystemInfo album) => GetAlbumSomething(album.Name, 1);

		private static string GetAlbumYear(FileSystemInfo album) => GetAlbumSomething(album.Name, 0);

		private static string GetAlbumSomething(string albumName, int index) =>
			_albumRegex.IsMatch(albumName)
				? _albumRegex.Split(albumName).Where(a => !string.IsNullOrWhiteSpace(a)).ToArray()[index]
				: index == 0 ? string.Empty : albumName;

		private static long GetArtistDirSizeInMb(DirectoryInfo artist)
		{
			return (artist?.EnumerateDirectories().Sum(GetAlbumDirSizeInMb) ?? 0)
			       + (artist?.EnumerateFiles().Sum(GetTrackSizeInMb) ?? 0);
		}

		private static long GetAlbumDirSizeInMb(DirectoryInfo album)
		{
			return album?.EnumerateFiles().Sum(GetTrackSizeInMb) ?? 0;
		}

		private static long GetTrackSizeInMb(FileInfo track)
		{
			return track.Length / 1024;
		}

	}
}
