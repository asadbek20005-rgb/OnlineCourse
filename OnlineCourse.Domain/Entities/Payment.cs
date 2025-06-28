using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("payments")]
public class Payment : Base
{
    [Column("user_id")]
    [Required]
    public Guid UserID { get; set; }
    [ForeignKey(nameof(UserID))]
    public User? User { get; set; }

    [Column("amount")]
    [Required]
    public decimal Amount { get; set; }

    [Column("course_id")]
    [Required]
    public int CourseId { get; set; }
    [ForeignKey(nameof(CourseId))]
    public Course? Course { get; set; }


    [Column("is_verified")]
    public bool IsVerified { get; set; } = false;


    [Column("payment_date")]
    [Required]
    public DateTime PaymentDate { get; set; }

}
