namespace OnlineCourse.Application.Models.Payment;

public class HasPaidRequestModel
{
    public Guid UserId { get; set; }
    public int CourseId { get; set; }
}
