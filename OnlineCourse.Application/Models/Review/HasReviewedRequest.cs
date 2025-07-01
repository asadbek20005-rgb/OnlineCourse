namespace OnlineCourse.Application.Models.Review;

public class HasReviewedRequest
{
    public Guid UserId { get; set; }
    public int CourseId { get; set; }
}
