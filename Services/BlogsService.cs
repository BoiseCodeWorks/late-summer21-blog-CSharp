using System;
using System.Collections.Generic;
using bloggr.Models;
using bloggr.Repositories;

namespace bloggr.Services
{
  public class BlogsService
  {
    private readonly BlogsRepository _repo;

    public BlogsService(BlogsRepository repo)
    {
      _repo = repo;
    }
    internal List<Blog> Get()
    {
      List<Blog> blogs = _repo.GetAll();
      return blogs.FindAll(b => b.Published == true);
    }
    internal Blog Get(int id, bool careIfPublished = true)
    {
      Blog blog = _repo.GetById(id);
      if (blog == null || (careIfPublished && blog.Published == false))
      {
        throw new Exception("Invalid Id");
      }
      return blog;
    }

    internal List<Blog> GetBlogsByCreator(string creatorId, bool careIfPublished = true)
    {
      List<Blog> blogs = _repo.GetAll(creatorId);
      if (careIfPublished)
      {
        blogs = blogs.FindAll(b => b.Published == true);
      }
      return blogs;
    }

    internal Blog Create(Blog newBlog)
    {
      return _repo.Create(newBlog);
    }

    internal Blog Update(Blog editedBlog)
    {
      Blog original = Get(editedBlog.Id, false);
      if (original.CreatorId != editedBlog.CreatorId)
      {
        throw new Exception("Invalid Access");
      }
      original.Body = editedBlog.Body.Length > 0 ? editedBlog.Body : original.Body;
      original.Title = editedBlog.Title.Length > 0 ? editedBlog.Title : original.Title;
      // null coalescing operator '?' ( aka Elvis Operator ) says if the value is null do not drill further
      // original.ImgUrl = editedBlog.ImgUrl?.Length > 0 ? editedBlog.ImgUrl : original.ImgUrl;
      original.ImgUrl = editedBlog.ImgUrl != null && editedBlog.ImgUrl.Length > 0 ? editedBlog.ImgUrl : original.ImgUrl;
      original.Published = editedBlog.Published != null ? editedBlog.Published : original.Published;
      return _repo.Update(original);
    }



    internal Blog Delete(int blogId, string userId)
    {
      Blog blogToDelete = Get(blogId, false);
      if (blogToDelete.CreatorId != userId)
      {
        throw new Exception("Invalid Access");
      }
      _repo.Delete(blogId);
      return blogToDelete;
    }
  }
}