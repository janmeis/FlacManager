namespace FlacManager.Models.Models
{
	public abstract class MusicBase
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public long Size { get; set; }
	}
}