using System.Collections.Generic;

namespace FlacManager.Models.Models
{
	public class Album: MusicBase
	{
		public Album()
		{
			Tracks = new List<AudioTrack>();
			Files = new List<File>();
		}
		public IEnumerable<AudioTrack> Tracks { get; set; }
		public IEnumerable<File> Files { get; set; }
	}
}