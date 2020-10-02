using System.IO;

namespace FlacManager.Models.Models
{
	public class File: MusicBase
	{
		public FileAttributes Attributes { get; set; }
		public string FullName { get; set; }
		public bool IsReadOnly { get; set; }
	}
}