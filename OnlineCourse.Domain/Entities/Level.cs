using Microsoft.EntityFrameworkCore;
using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;

[Table("levels")]
[Index(nameof(Name), IsUnique = true)]
public class Level : Base
{
    [Column("name")]
    [Required]
    public string Name { get; set; } = string.Empty;
}
