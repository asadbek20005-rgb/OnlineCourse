using Microsoft.AspNetCore.Http;

namespace OnlineCourse.Application.Models.Course;

public class UploadCourseImgModel
{
    public Guid UserId { get; set; }
    public int CourseId { get; set; }

    public IFormFile File { get; set; }
}
