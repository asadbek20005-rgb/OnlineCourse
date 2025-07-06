using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("post")]
public class Post : Base
{
    [Required]
    [Column("title")]
    public string Title { get; set; }
    [Required]
    [Column("contents")]
    public List<string>? Contents { get; set; } = new List<string>();
    [Column("post_photo_option")]
    public virtual List<PostPhotoOption> PhotoList { get; set; }
    
    [Column("file_url")]
    public string? FileUrl { get; set; }
    [Required]
    public string AuthorFullName { get; set; }

    public int BlogId { get; set; }
    public virtual Blog? Blog { get; set; }

}
