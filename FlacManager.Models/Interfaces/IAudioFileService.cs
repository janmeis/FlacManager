using FlacManager.Models.Models;
using System.Collections.Generic;
using System.IO;

namespace FlacManager.Models.Interfaces
{
	public interface IAudioFileService
	{
		IEnumerable<AudioTrack> GetTracks(DirectoryInfo album);
	}
}
