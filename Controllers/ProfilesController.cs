using System;
using System.Collections.Generic;
using bloggr.Models;
using bloggr.Services;
using Microsoft.AspNetCore.Mvc;

namespace bloggr.Controllers
{
  [ApiController]
  [Route("/api/[controller]")]
  public class ProfilesController : ControllerBase
  {
    private readonly BlogsService _bs;
    private readonly AccountService _acs;

    public ProfilesController(BlogsService bs, AccountService acs)
    {
      _bs = bs;
      _acs = acs;
    }

    [HttpGet("{id}")]
    public ActionResult<Profile> Get(string id)
    {
      try
      {
        Profile prof = _acs.GetProfileById(id);
        return prof;
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpGet("{id}/blogs")]
    public ActionResult<List<Blog>> GetBlogs(string id)
    {
      try
      {
        List<Blog> blogs = _bs.GetBlogsByCreator(id);
        return blogs;
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }
  }
}