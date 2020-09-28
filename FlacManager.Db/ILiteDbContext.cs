using LiteDB;

namespace FlacManager.Db
{
    public interface ILiteDbContext
    {
        LiteDatabase Database { get; }
    }
}