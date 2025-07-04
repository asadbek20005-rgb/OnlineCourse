using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("otps")]
public class EmailOtp : Base
{
    [Column("email")]
    [Required]
    public string Email { get; set; }

    [Column("code")]
    [Required]
    public int Code { get; set; }

    [Column("is_expired")]
    public bool IsExpired { get; set; } = false;
}
