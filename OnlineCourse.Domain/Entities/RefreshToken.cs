using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("refresh_token")]
public class RefreshToken
{
    [Column("id")]
    [Key]
    public int Id { get; set; }
    [Column("token")]
    [Required]
    public string Token { get; set; } = string.Empty;
    [Column("expires")]
    [Required]
    public DateTime Expires { get; set; }
    [Column("is_revoked")]
    public bool IsRevoked { get; set; } = false;
    [Column("user_id")]
    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

}