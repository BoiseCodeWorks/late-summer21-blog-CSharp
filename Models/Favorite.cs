using System.ComponentModel.DataAnnotations;

namespace bloggr.Models
{
  // NOTE MANY TO MANY MODEL
  public class Favorite
  {
    public int Id { get; set; }
    public string AccountId { get; set; }
    [Required]
    public int BlogId { get; set; }
  }
}