using FlacManager.Models;
using System.Collections.Generic;

namespace FlacManager.Db.LiteDb
{
	public interface ILiteDbMusicCatalogService
	{
        int Delete(int id);
        int DeleteCollection();
        IEnumerable<Artist> FindAll();
        Artist FindOne(int id);
        int Insert(Artist artist);
        bool Update(Artist artist);
    }
}
