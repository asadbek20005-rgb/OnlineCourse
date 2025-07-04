using Microsoft.EntityFrameworkCore;
using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("users")]
[Index(nameof(Email), IsUnique = true)]
public class User : Date
{
    [Column("id")]
    [Key]
    public Guid Id { get; set; }

    [Column("full_name")]
    [Required]
    [MaxLength(70)]
    [MinLength(3)]
    [RegularExpression("^[A-Z][a-z]+(?:\\s[A-Z][a-z]+)+$\r\n")]
    public string FullName { get; set; } = string.Empty;

    [Column("email")]
    [Required, EmailAddress]
    [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$\r\n")]
    public string Email { get; set; } = string.Empty;

    [Column("username")]
    [Required]
    [RegularExpression("^(?![0-9])[a-zA-Z0-9](?!.*[_.]{2})[a-zA-Z0-9._]{2,19}$\r\n")]
    public string UserName { get; set; } = string.Empty;

    [Column("password_hash")]
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("role")]
    [Required]
    public UserRole Role { get; set; }


    [Column("status")]
    [Required]
    public UserStatus Status { get; set; }

    [Column("img_url")]
    public string? ImgUrl { get; set; }


    [Column("last_login")]
    public DateTime? LastLogin { get; set; }

    [Column("email_confirmation")]
    public bool EmailConfirmed { get; set; } = false;

    public virtual Instructor? Instructor { get; set; }
    public virtual Student? Student { get; set; }

    public virtual ICollection<Notification>? Notification { get; set; }

    [Column("is_blocked")]
    public bool IsBlocked { get; set; } = false; 



}



public enum UserRole { Admin, Instructor, Student }
public enum UserStatus { Active, Deactivated }
