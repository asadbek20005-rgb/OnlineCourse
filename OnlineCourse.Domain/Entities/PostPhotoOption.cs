using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("post_photo_option")]
public class PostPhotoOption : Base
{
    [Column("photo_url")]
    public string? PhotoUrl { get; set; }
    [Column("public_id")]
    public string? PublicId { get; set; }
    [Column("post_id")]
    public int PostId { get; set; }
    public virtual Post? Post { get; set; }
}
