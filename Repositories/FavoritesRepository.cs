using System.Data;
using bloggr.Models;
using Dapper;

namespace bloggr.Repositories
{
  public class FavoritesRepository
  {
    private readonly IDbConnection _db;

    public FavoritesRepository(IDbConnection db)
    {
      _db = db;
    }

    internal Favorite Create(Favorite newFavorite)
    {
      string sql = @"
      INSERT INTO favorites
      (accountId, blogId)
      VALUES
      (@AccountId, @BlogId);
      SELECT LAST_INSERT_ID();
      ";
      newFavorite.Id = _db.ExecuteScalar<int>(sql, newFavorite);
      return newFavorite;
    }

    internal Favorite GetById(int id)
    {
      string sql = "SELECT * FROM favorites WHERE id = @id";
      return _db.QueryFirstOrDefault<Favorite>(sql, new { id });
    }

    internal void Delete(int id)
    {
      string sql = "DELETE FROM favorites WHERE id = @id LIMIT 1";
      _db.Execute(sql, new { id });
    }
  }
}