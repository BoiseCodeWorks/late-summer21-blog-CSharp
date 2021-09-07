using System.ComponentModel.DataAnnotations;

namespace bloggr.Models
{
  public class Blog
  {
    public int Id { get; set; }
    [Required]
    [MaxLength(30)]
    public string Title { get; set; }
    [Required]
    public string Body { get; set; }
    public string ImgUrl { get; set; }
    // NOTE the '?' allows non nullable properties to be null (bool, number types)
    public bool? Published { get; set; }
    public string CreatorId { get; set; }
    // acts as a virtual 
    public Profile Creator { get; set; }
  }
}