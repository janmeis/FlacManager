using FlacManager.Db.LiteDb;
using FlacManager.Models;
using FlacManager.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            var documentPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Documents";
            services.AddSingleton<IDocumentPath, DocumentPath>(s => new DocumentPath { Path = documentPath });

            services.Configure<LiteDbOptions>(Configuration.GetSection("LiteDbOptions"));
            services.Configure<MusicLibraryOptions>(Configuration.GetSection("MusicLibraryOptions"));

            services.AddSingleton<ILiteDbContext, LiteDbContext>();
            services.AddTransient<ILiteDbMusicCatalogService, LiteDbMusicCatalogService>();

            services.AddTransient<IHandleArtists, HandleArtists>();
        }
    }
}
