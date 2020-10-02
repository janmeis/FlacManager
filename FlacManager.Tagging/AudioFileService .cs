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

		public IEnumerable<AudioTrack> GetTracks(DirectoryInfo album)
		{
			var audioTracks = new List<AudioTrack>();
			foreach (var cueFile in album.EnumerateFiles("*.cue").Select(file => file.FullName))
			{
				try
				{
					var cueContent = CatalogDataReaderFactory.GetInstance().GetCatalogDataReader(cueFile);
					var comments = cueContent.Comments.Split('˵', StringSplitOptions.RemoveEmptyEntries)
						.ToDictionary(c => c.Substring(0, c.IndexOf(' ')), c => c.Substring(c.IndexOf(' ') + 1).Replace("\"", ""));
					var cueTracks = cueContent.Tracks.Select(track => new AudioTrack(track)
					{
						Genre = comments["GENRE"],
						Year = int.Parse(comments["DATE"]),
						Comment = comments["COMMENT"]
					});

					audioTracks.AddRange(cueTracks);
				}
				catch (Exception e)
				{
					Console.WriteLine(cueFile);
					Console.WriteLine("******");
					Console.WriteLine(e);
				}
			}

			if (audioTracks.Count > 0)
				return audioTracks;


			return album.EnumerateFiles()
				.Where(file => _extensionsRegex.IsMatch(Path.GetExtension(file.FullName)))
				.Select(file =>
				{
					var audioTrack = new AudioTrack { Title = file.Name };
					try
					{
						var track = new Track(file.FullName);
						audioTrack = new AudioTrack(track);
					}
					catch (Exception e)
					{
						Console.WriteLine(file.FullName);
						Console.WriteLine("******");
						Console.WriteLine(e);
					}

					return audioTrack;
				});
		}
	}
}
