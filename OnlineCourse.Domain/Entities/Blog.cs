using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("blog")]
public class Blog : Base
{

    [Required]
    [Column("name")]
    public string Name { get; set; }
    [Required]
    [Column("description")]
    public string Description { get; set; }
    [Column("photo_url")]
    public string? PhotoUrl { get; set; }
    [Column("photo_public_id")]
    public string? PhotoPublicId { get; set; }

    public Guid UserId { get; set; }
    public virtual User? User { get; set; }

    public List<Post> Posts { get; set; }
}
