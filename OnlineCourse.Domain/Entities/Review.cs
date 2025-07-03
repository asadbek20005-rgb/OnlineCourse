using Microsoft.EntityFrameworkCore;
using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("reviews")]
[Index(nameof(UserID), nameof(CourseId), IsUnique = true)]
public class Review : Base
{
    [Column("user_id")]
    [Required]
    public Guid UserID { get; set; }
    [ForeignKey(nameof(UserID))]
    public virtual User? User { get; set; }


    [Column("course_id")]
    [Required]
    public int CourseId { get; set; }
    [ForeignKey(nameof(CourseId))]
    public virtual Course? Course { get; set; }


    [Column("comment")]
    [Required]
    [MaxLength(100)]
    [MinLength(1)]
    [RegularExpression("^[\\s\\S]{3,500}$\r\n")]
    public string Comment { get; set; } = string.Empty;



    [Column("rating")]
    [Range(1, 5)]
    public int Rating { get; set; }


    [Column("has_reviewed")]
    public bool HasReviewed { get; set; } = false;

}
