using System;
using bloggr.Models;
using bloggr.Repositories;

namespace bloggr.Services
{
  public class FavoritesService
  {
    private readonly FavoritesRepository _repo;

    public FavoritesService(FavoritesRepository repo)
    {
      _repo = repo;
    }

    internal Favorite Create(Favorite newFavorite)
    {
      // TODO Check if blog is published
      return _repo.Create(newFavorite);
    }

    private Favorite GetById(int id)
    {
      Favorite found = _repo.GetById(id);
      if (found == null)
      {
        throw new Exception("Invalid Id");
      }
      return found;
    }

    internal void Delete(int id, string userId)
    {
      Favorite favToDelete = GetById(id);
      if (favToDelete.AccountId != userId)
      {
        throw new Exception("Invalid Access");
      }
      _repo.Delete(id);
    }
  }
}