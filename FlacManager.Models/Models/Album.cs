using System.Collections.Generic;

namespace FlacManager.Models.Models
{
	public class Album: MusicBase
	{
		public Album()
		{
			Tracks = new List<AudioTrack>();
		}
		public IEnumerable<AudioTrack> Tracks { get; set; }
	}
}