using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.Category;

public class CreateCategoryModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

}
