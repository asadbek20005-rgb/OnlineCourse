using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("blogs")]
public class Blog : Base
{
    [Column("title")]
    [MaxLength(100)]
    [MinLength(3)]
    [Required]
    public string Title { get; set; } = string.Empty;

    [Column("details")]
    [MaxLength(250)]
    [MinLength(10)]
    public string? Details {  get; set; }

    [Column("img_url")]
    public string? ImgUrl { get; set; }

    public virtual ICollection<Comment>? Comments { get; set; }
    [Column("user_id")]
    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }  
}
