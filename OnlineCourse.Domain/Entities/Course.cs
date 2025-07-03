using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;

[Table("courses")]
public class Course : Base
{
    [Column("instructor_id")]
    public int InstructorId { get; set; }
    [ForeignKey(nameof(InstructorId))]
    public virtual Instructor? Instructor { get; set; }

    [Column("title")]
    [Required]
    [MaxLength(50)]
    [MinLength(3)]
    [RegularExpression("^(?!\\s)[a-zA-Z0-9?-??-???\\s\\-:',.&()]{3,100}(?<!\\s)$\r\n")]
    public string Title { get; set; } = string.Empty;


    [Column("category_id")]
    public int CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public virtual Category? Category { get; set; }


    [Column("level_id")]
    public int LevelId { get; set; }

    [ForeignKey(nameof(LevelId))]
    public virtual Level? Level { get; set; }


    [Column("cover_img_url")]
    public string? CoverImgUrl { get; set; }

    [Column("price")]
    [Required]
    public decimal Price { get; set; }


    [Column("is_published")]
    public bool IsPublished { get; set; } = false;

    [Column("rating")]
    public decimal? Rating { get; set; }

    [Column("has_completed")]
    public bool HasCompleted { get; set; } = false;

    public virtual ICollection<Lesson>? Lessons { get; set; }
    public virtual ICollection<Review>? Reviews { get; set; }
    public virtual ICollection<Favourite>? Favourites { get; set; }
    public virtual ICollection<StudentProgress>? StudentProgresses { get; set; }
    public virtual ICollection<Payment>? Payments { get; set; }
    public virtual ICollection<Enrollment>? Enrollments { get; set; }
}
