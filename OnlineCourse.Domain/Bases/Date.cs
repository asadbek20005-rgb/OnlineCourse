using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Bases;

public abstract class Date
{
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
