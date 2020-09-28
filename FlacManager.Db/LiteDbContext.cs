using System.IO;
using FlacManager.Models.Interfaces;
using FlacManager.Models.Models;
using LiteDB;
using Microsoft.Extensions.Options;

namespace FlacManager.Db
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
