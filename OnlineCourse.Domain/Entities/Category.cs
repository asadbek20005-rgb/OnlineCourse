using Microsoft.EntityFrameworkCore;
using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("categories")]
[Index(nameof(Name), IsUnique = true)]
public class Category : Base
{
    [Column("name")]
    [Required]
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<Course>? Courses { get; set; }

}
