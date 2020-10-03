using FlacManager.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace FlacManager.ConsoleApp
{
	/// <see cref= "https://jameshobday.dev/2020/04/22/adding-a-json-configuration-file-to-a-net-core-console-application/" />
	/// <see cref= "https://stackoverflow.com/a/41411310" />
	internal class Program
	{
		private static void Main(string[] args)
		{
			IServiceCollection services = new ServiceCollection();
			var startup = new Startup();
			startup.ConfigureServices(services);
			IServiceProvider serviceProvider = services.BuildServiceProvider();

			// Get Service and call method
			// ReSharper disable once InconsistentNaming
			var _handleArtists = serviceProvider.GetService<IHandleArtists>();

			DateTime? from = null;
			DateTime? to = null;
			if (args.Length > 0) from = DateTime.Parse(args[0]);
			if (args.Length > 1) to = DateTime.Parse(args[1]);

			Console.WriteLine("---");

			_handleArtists.DeleteAll();
			_handleArtists.StoreArtists(from, to);

			//var tracks = _handleArtists.EnumerateArtists()
			//	.SelectMany(a => a.Albums)
			//	.SelectMany(t => t.Tracks).ToList();
			//tracks.ForEach(t =>
			//{
			//	Console.WriteLine($"artist:{t.Artist}, album:{t.Album}, year:{t.Year}, trackNumber:{t.TrackNumber}, title:{t.Title}, duration: {TimeSpan.FromSeconds(t.Duration):hh\\:mm\\:ss}");
			//});

			Console.WriteLine("---");
			Console.ReadLine();
		}
	}
}
