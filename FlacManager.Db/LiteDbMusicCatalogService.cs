using FlacManager.Models;
using LiteDB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FlacManager.Db.LiteDb
{
	public class LiteDbMusicCatalogService : ILiteDbMusicCatalogService
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

		public int DeleteCollection()
		{
			var col = _liteDb.GetCollection<Artist>("artists");
			return col.DeleteAll();
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
	}
}
