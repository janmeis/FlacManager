using LiteDB;

namespace FlacManager.Db.LiteDb
{
    public interface ILiteDbContext
    {
        LiteDatabase Database { get; }
    }
}