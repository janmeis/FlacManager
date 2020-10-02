using System;
using System.Collections.Generic;
using System.Linq;
using FlacManager.Models.Interfaces;
using FlacManager.Models.Models;
using LiteDB;

namespace FlacManager.Db
{
	public class LiteDbMusicCatalogService : IMusicCatalogService
	{
		private readonly LiteDatabase _liteDb;

		public LiteDbMusicCatalogService(ILiteDbContext liteDbContext)
		{
			_liteDb = liteDbContext.Database;
		}

		public int Delete(int id)
		{
			return _liteDb.GetCollection<Artist>("artists")
				.DeleteMany(x => x.Id == id);
		}

		public bool DeleteAll()
		{
			return _liteDb.DropCollection("artists")
				&& _liteDb.DropCollection("artists_created");
		}

		public IEnumerable<Artist> FindAll()
		{
			var result = _liteDb.GetCollection<Artist>("artists")
				.FindAll();
			return result;
		}

		public Artist FindOne(int id)
		{
			return _liteDb.GetCollection<Artist>("artists")
				.Find(x => x.Id == id).FirstOrDefault();
		}

		public int Insert(Artist artist)
		{
			return _liteDb.GetCollection<Artist>("artists")
				.Insert(artist);
		}

		public bool Update(Artist artist)
		{
			return _liteDb.GetCollection<Artist>("artists")
				.Update(artist);
		}

		public void CreateStatistics()
		{
			var statistics = new Statistics { Created = DateTime.Now };
			_liteDb.GetCollection<Statistics>("artists_created")
				.Insert(statistics);
		}

		public void SetIndexes()
		{
			var col = _liteDb.GetCollection<Artist>("artists");
			col.EnsureIndex(x => x.Name);
		}
	}
}
