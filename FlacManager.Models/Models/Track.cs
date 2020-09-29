using System;

namespace FlacManager.Models.Models
{
	public class Track : MusicBase
	{
		public int Number { get; set; }
		public string Format { get; set; }
		public DateTime Length { get; set; }
	}
}