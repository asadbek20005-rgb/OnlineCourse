namespace OnlineCourse.Application.Models.Favourite;

public class IsFavouriteRequestModel
{
    public Guid UserId { get; set; }
    public int CourseId { get; set; }
}
