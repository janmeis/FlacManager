using System;

namespace FlacManager.Models.Models
{
	public abstract class MusicBase
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public long Length { get; set; }
		public DateTime CreationTime { get; set; }
		public DateTime LastWriteTime { get; set; }
	}
}