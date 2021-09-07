using System;
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
  [Authorize]
  public class FavoritesController : ControllerBase
  {
    private readonly FavoritesService _fs;

    public FavoritesController(FavoritesService fs)
    {
      _fs = fs;
    }

    [HttpPost]
    public async Task<ActionResult<Favorite>> Create([FromBody] Favorite newFavorite)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        newFavorite.AccountId = userInfo.Id;
        Favorite created = _fs.Create(newFavorite);
        return Ok(created);
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<String>> Delete(int id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        _fs.Delete(id, userInfo.Id);
        return Ok("Delorted");
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }
  }
}