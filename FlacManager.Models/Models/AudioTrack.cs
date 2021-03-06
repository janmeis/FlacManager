﻿using System.IO;
using ATL;

namespace FlacManager.Models.Models
{
	public class AudioTrack : MusicBase
	{
		public AudioTrack()
		{
		}

		public AudioTrack(Track track)
		{
			Path = track.Path;
			Title = track.Title;
			Artist = track.Artist;
			Composer = track.Composer;
			Comment = track.Comment;
			Genre = track.Genre;
			Album = track.Album;
			AlbumArtist = track.AlbumArtist;
			Year = $"{track.Year}";
			TrackNumber = track.TrackNumber;
			TrackTotal = track.TrackTotal;
			DiscNumber = track.DiscNumber;
			DiscTotal = track.DiscTotal;
			Bitrate = track.Bitrate;
			SampleRate = track.SampleRate;
			Duration = track.Duration;

			IsCue = false;
		}

		public AudioTrack(Track track, FileInfo file) : this(track)
		{
			Name = file.Name;
			Length = file.Length;
			CreationTime = file.CreationTime;
			LastWriteTime = file.LastWriteTime;
		}

		public string Path { get; set; }
		public string Title { get; set; }
		public string Artist { get; set; }
		public string Composer { get; set; }
		public string Comment { get; set; }
		public string Genre { get; set; }
		public string Album { get; set; }
		public string AlbumArtist { get; set; }
		public string Year { get; set; }
		public int TrackNumber { get; set; }
		public int TrackTotal { get; set; }
		public int DiscNumber { get; set; }
		public int DiscTotal { get; set; }
		// ReSharper disable once IdentifierTypo
		public int Bitrate { get; set; }
		public double SampleRate { get; set; }
		public int Duration { get; set; }

		public bool IsCue { get; set; }
	}
}