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
  [Route("[controller]")]
  public class AccountController : ControllerBase
  {
    private readonly AccountService _accountService;
    private readonly BlogsService _bs;

    public AccountController(AccountService accountService, BlogsService bs)
    {
      _accountService = accountService;
      _bs = bs;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<Account>> Get()
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        return Ok(_accountService.GetOrCreateProfile(userInfo));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }


    [HttpGet("blogs")]
    [Authorize]
    public async Task<ActionResult<List<Blog>>> GetBlogs()
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        List<Blog> blogs = _bs.GetBlogsByCreator(userInfo.Id, false);
        return Ok(blogs);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpGet("blogs/favorites")]
    [Authorize]
    public async Task<ActionResult<List<BlogFavoriteViewModel>>> GetFavoriteBlogs()
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        List<BlogFavoriteViewModel> blogs = _bs.GetFavoriteBlogsByAccount(userInfo.Id);
        return Ok(blogs);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }


}