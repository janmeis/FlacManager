using FlacManager.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace FlacManager.ConsoleApp
{
	/// <see cref= "https://jameshobday.dev/2020/04/22/adding-a-json-configuration-file-to-a-net-core-console-application/" />
	/// <see cref= "https://stackoverflow.com/a/41411310" />
	class Program
	{
		static void Main(string[] args)
		{
			IServiceCollection services = new ServiceCollection();
			Startup startup = new Startup();
			startup.ConfigureServices(services);
			IServiceProvider serviceProvider = services.BuildServiceProvider();

			// Get Service and call method
			var handleArtists = serviceProvider.GetService<IHandleArtists>();
			handleArtists.StoreArtists();
			var artists = handleArtists.EnumerateArtists().ToList();
			artists.ForEach(a => Console.WriteLine(a.Name));

			Console.WriteLine("---");
			Console.ReadLine();
		}
	}
}
