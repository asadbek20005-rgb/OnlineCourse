namespace OnlineCourse.Application.Models.Course;

public class GetCoursesByPriceModel
{
    public int InstructorId { get; set; }
    public decimal InitialPrice { get; set; }
    public decimal LastPrice { get; set; }
}
