using OnlineCourse.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Application.Dtos;

public class BlogDto
{
  
    public string Title { get; set; } = string.Empty;

    public string Details { get; set; }

    public string? ImgUrl { get; set; }

    public Guid UserId { get; set; }
}
