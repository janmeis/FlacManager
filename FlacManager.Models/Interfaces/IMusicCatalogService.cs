using System.Collections.Generic;
using FlacManager.Models.Models;

namespace FlacManager.Models.Interfaces
{
	public interface IMusicCatalogService
	{
        int Delete(int id);
        bool DeleteAll();
        IEnumerable<Artist> FindAll();
        Artist FindOne(int id);
        int Insert(Artist artist);
        bool Update(Artist artist);
    }
}
