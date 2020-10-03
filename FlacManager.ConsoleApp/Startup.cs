using FlacManager.Db;
using FlacManager.Models.Interfaces;
using FlacManager.Models.Models;
using FlacManager.Service;
using FlacManager.Tagging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace FlacManager.ConsoleApp
{
	public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json");
            
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
	        services.AddLogging(logging =>
	        {
		        var loggingSection = Configuration.GetSection("Logging");
		        logging.AddConfiguration(loggingSection);
		        logging.AddFile(loggingSection);
            });

            var documentPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Documents";
            services.AddSingleton<IDocumentPath, DocumentPath>(s => new DocumentPath { Path = documentPath });

            services.Configure<LiteDbOptions>(Configuration.GetSection("LiteDbOptions"));
            services.Configure<MusicLibraryOptions>(Configuration.GetSection("MusicLibraryOptions"));

            services.AddSingleton<ILiteDbContext, LiteDbContext>();
            services.AddTransient<IMusicCatalogService, LiteDbMusicCatalogService>();
            services.AddTransient<IAudioFileService, AudioFileService >();

            services.AddTransient<IHandleArtists, HandleArtists>();
        }
    }
}
