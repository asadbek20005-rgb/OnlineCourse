namespace OnlineCourse.Application.Dtos;

public class FavouriteDto
{
    public int Id { get; set; }
    public Guid UserID { get; set; }
    public int CourseId { get; set; }
}
