using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Application.Models.Log;

public class GetByTargetRequestModel
{
    public string TargetId { get; set; } = string.Empty;
    public ActivityTargetType Target { get; set; }

}
