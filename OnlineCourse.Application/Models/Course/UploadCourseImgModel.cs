using Microsoft.AspNetCore.Http;
using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Application.Models.Course;

public class UploadCourseImgModel
{
    public int InstructorId { get; set; }
    public int CourseId { get; set; }

    public IFormFile File { get; set; }
}
