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
    public virtual User? User { get; set; }

    [Column("amount")]
    [Required]
    public decimal Amount { get; set; }

    [Column("course_id")]
    [Required]
    public int CourseId { get; set; }
    [ForeignKey(nameof(CourseId))]
    public virtual Course? Course { get; set; }


    [Column("is_verified")]
    public bool IsVerified { get; set; } = false;


    [Column("payment_date")]
    [Required]
    public DateTime PaymentDate { get; set; }

    [Column("has_paid")]
    public bool HasPaid { get; set; } = false;

}
