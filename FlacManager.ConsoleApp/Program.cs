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

			DateTime? from = null;
			DateTime? to = null;
			if (args.Length > 0) from = DateTime.Parse(args[0]);
			if (args.Length > 1) to = DateTime.Parse(args[1]);

			Console.WriteLine("---");

			// Get Service and call method
			var handleArtists = serviceProvider.GetService<IHandleArtists>();
			handleArtists.DeleteAll();
			handleArtists.StoreArtists(from, to);
			var artists = handleArtists.EnumerateArtists().ToList();
			artists.ForEach(artist =>
			{
				Console.WriteLine($"artist:{artist.Name}, size:{artist.Length / 1024:N0}, created:{artist.CreationTime:d}");
				artist.Albums.ToList().ForEach(album =>
				{
					Console.WriteLine($"\tname:{album.Name}, size:{album.Length / 1024:N0}, created:{album.CreationTime:d}");
					album.Files.ToList().ForEach(file => Console.WriteLine($"\t\tname:{file.Name}, size:{file.Length / 1024:N0}, created:{file.CreationTime:d}"));
				});
			});

			Console.WriteLine("---");
			Console.ReadLine();
		}
	}
}
