using System.Collections.Generic;

namespace FlacManager.Models.Models
{
	public class Artist: MusicBase
	{
		public Artist()
		{
			Albums = new List<Album>();
		}
		public IEnumerable<Album> Albums { get; set; }
	}
}
