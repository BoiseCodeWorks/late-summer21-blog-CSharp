using System.Collections.Generic;
using System.Data;
using System.Linq;
using bloggr.Models;
using Dapper;

namespace bloggr.Repositories
{
  public class BlogsRepository
  {
    private readonly IDbConnection _db;

    public BlogsRepository(IDbConnection db)
    {
      _db = db;
    }


    internal List<Blog> GetAll()
    {
      string sql = @"
      SELECT 
        a.*,
        b.*
      FROM blogs b
      JOIN accounts a ON a.id = b.creatorId;
      ";
      return _db.Query<Profile, Blog, Blog>(sql, (prof, blog) =>
      {
        blog.Creator = prof;
        return blog;
      }, splitOn: "id").ToList<Blog>();
    }

    internal Blog GetById(int id)
    {
      string sql = @"
      SELECT 
        a.*,
        b.*
      FROM blogs b
      JOIN accounts a ON a.id = b.creatorId
      WHERE b.id = @id;
      ";
      return _db.Query<Profile, Blog, Blog>(sql, (prof, blog) =>
      {
        blog.Creator = prof;
        return blog;
      }, new { id }, splitOn: "id").FirstOrDefault();
    }

    internal List<Blog> GetAll(string creatorId)
    {
      string sql = @"
      SELECT 
        a.*,
        b.*
      FROM blogs b
      JOIN accounts a ON a.id = b.creatorId
      WHERE b.creatorId = @creatorId;
      ";
      return _db.Query<Profile, Blog, Blog>(sql, (prof, blog) =>
      {
        blog.Creator = prof;
        return blog;
      }, new { creatorId }, splitOn: "id").ToList<Blog>();
    }

    internal Blog Create(Blog newBlog)
    {
      string sql = @"
        INSERT INTO blogs
        (title, body, imgUrl, published, creatorId)
        VALUES
        (@Title, @Body, @ImgUrl, @Published, @CreatorId);
        SELECT LAST_INSERT_ID();
        ";
      int id = _db.ExecuteScalar<int>(sql, newBlog);
      return GetById(id);
    }

    internal Blog Update(Blog original)
    {
      string sql = @"
        UPDATE blogs
        SET
            title = @Title,
            body = @Body, 
            imgUrl = @ImgUrl,
            published = @Published
        WHERE id = @Id;
      ";
      _db.Execute(sql, original);
      return GetById(original.Id);
    }

    internal List<BlogFavoriteViewModel> GetFavoriteBlogsByAccount(string id)
    {
      // MANY TO MANY GET
      // FROM is always the many to many table
      // WHERE column from the many to many table
      // JOIN collection you want to retrieve ON col from m-2-m match to collection to retreive
      string sql = @"
      SELECT
        a.*,
        b.*,
        f.id AS favoriteId
      FROM favorites f
      JOIN blogs b ON f.blogId = b.id
      JOIN accounts a ON b.creatorId = a.id
      WHERE f.accountId = @id
      ";
      return _db.Query<Profile, BlogFavoriteViewModel, BlogFavoriteViewModel>(sql, (prof, blog) =>
      {
        blog.Creator = prof;
        return blog;
      }, new { id }, splitOn: "id").ToList<BlogFavoriteViewModel>();
    }

    internal void Delete(int id)
    {
      string sql = "DELETE FROM blogs WHERE id = @id LIMIT 1;";
      _db.Execute(sql, new { id });
    }
  }
}