using System.Collections.Generic;

namespace FlacManager.Models.Models
{
	public class Album: MusicBase
	{
		public Album()
		{
			Tracks = new List<Track>();
		}
		public string Year { get; set; }
		public IEnumerable<Track> Tracks { get; set; }
	}
}