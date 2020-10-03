using ATL;
using ATL.CatalogDataReaders;
using FlacManager.Models.Interfaces;
using FlacManager.Models.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FlacManager.Tagging
{
	public class AudioFileService : IAudioFileService
	{
		private static readonly Regex _extensionsRegex = new Regex(@"^(\.flac)|(\.mp3)|(\.m4a)|(\.ape)$", RegexOptions.IgnoreCase);
		// ReSharper disable once IdentifierTypo
		private static readonly Regex _losslessFileRegex = new Regex(@"FILE\s+""(.+)""\s+WAVE", RegexOptions.Multiline);

		public IEnumerable<AudioTrack> GetTracks(DirectoryInfo album)
		{
			var index = 1;
			if (!album.EnumerateFiles("*.cue").Any())
				return album.EnumerateFiles()
					.Where(file => _extensionsRegex.IsMatch(Path.GetExtension(file.FullName)))
					.Select(file =>
					{
						var audioTrack = new AudioTrack { Name = file.Name };
						try
						{
							var track = new Track(file.FullName);
							audioTrack = new AudioTrack(track, file)
							{
								Id = index++
							};
						}
						catch (Exception e)
						{
							Console.WriteLine(file.FullName);
							Console.WriteLine("******");
							Console.WriteLine(e);
						}

						return audioTrack;
					});

			var audioTracks = new List<AudioTrack>();
			foreach (var cueFile in album.EnumerateFiles("*.cue").Select(file => file.FullName))
			{
				try
				{
					var cueFileContent = File.ReadAllText(cueFile);
					var cueFileSplit = _losslessFileRegex.Split(cueFileContent);
					// ReSharper disable once IdentifierTypo
					var losslessFile = Path.Combine(album.FullName, cueFileSplit.Length > 1 ? cueFileSplit[1] : "fileNotFound");
					if (!File.Exists(losslessFile))
						throw new FileNotFoundException(losslessFile);

					var cueContent = CatalogDataReaderFactory.GetInstance().GetCatalogDataReader(cueFile);
					var comments = cueContent.Comments.Split('˵', StringSplitOptions.RemoveEmptyEntries)
						.ToDictionary(c => c.Substring(0, c.IndexOf(' ')),
							c => c.Substring(c.IndexOf(' ') + 1).Replace("\"", ""));
					var cueTracks = cueContent.Tracks.Select(track => new AudioTrack(track, new FileInfo(losslessFile))
					{
						Id = index++,
						IsCue = true,

						Genre = comments.ContainsKey("GENRE") ? comments["GENRE"] : string.Empty,
						Year = comments.ContainsKey("DATE") ? comments["DATE"] : string.Empty,
						Comment = comments.ContainsKey("COMMENT") ? comments["COMMENT"] : string.Empty
					});

					audioTracks.AddRange(cueTracks);
				}
				catch (FileNotFoundException notFoundException)
				{
					Console.WriteLine($"File not found: {notFoundException.Message}");
					Console.WriteLine("******");
				}
				catch (Exception exception)
				{
					Console.WriteLine(cueFile);
					Console.WriteLine("******");
					Console.WriteLine(exception);
				}
			}

			return audioTracks;
		}
	}
}
