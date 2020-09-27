using FlacManager.Models;
using LiteDB;
using Microsoft.Extensions.Options;
using System.IO;

namespace FlacManager.Db.LiteDb
{
	public class LiteDbContext : ILiteDbContext
    {
        public LiteDatabase Database { get; }

		public LiteDbContext(IOptions<LiteDbOptions> options, IDocumentPath documentPath)
        {
            var path = Path.Combine(documentPath.Path, options.Value.DatabaseLocation);
            Database = new LiteDatabase(path);
        }
    }
}
