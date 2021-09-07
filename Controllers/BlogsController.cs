using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bloggr.Models;
using bloggr.Services;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bloggr.Controllers
{
  [ApiController]
  [Route("/api/[controller]")]
  public class BlogsController : ControllerBase
  {
    private readonly BlogsService _bs;

    public BlogsController(BlogsService service)
    {
      _bs = service;
    }

    [HttpGet]
    public ActionResult<List<Blog>> Get()
    {
      try
      {
        List<Blog> blogs = _bs.Get();
        return Ok(blogs);
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpGet("{id}")]
    public ActionResult<Blog> Get(int id)
    {
      try
      {
        Blog blog = _bs.Get(id);
        return Ok(blog);
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Blog>> Create([FromBody] Blog newBlog)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        newBlog.CreatorId = userInfo.Id;
        Blog created = _bs.Create(newBlog);
        return Ok(created);
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<Blog>> Update(int id, [FromBody] Blog editedBlog)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        editedBlog.CreatorId = userInfo.Id;
        editedBlog.Id = id;
        Blog blog = _bs.Update(editedBlog);
        return Ok(blog);
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<Blog>> Delete(int id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        Blog blog = _bs.Delete(id, userInfo.Id);
        return Ok(blog);
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }




  }
}